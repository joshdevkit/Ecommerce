using Ecommerce.Application.Commands.Auth;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Interfaces.Admin;
using Ecommerce.Application.Interfaces.Auth;
using Ecommerce.Application.Validators;
using Ecommerce.Application.Validators.Behaviors;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repositories;
using Ecommerce.Infrastructure.Repositories.Admin;
using Ecommerce.Infrastructure.Repositories.Auth;
using Ecommerce.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ecommerce.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection")
                                     ?? throw new ArgumentNullException(nameof(configuration), "DefaultConnection string is missing");

            // Register DBConnection as transient because it is a lightweight wrapper around SqlConnection,
            // and we want to ensure that each repository gets a new instance of DBConnection to manage its own connection and transaction scope.
            services.AddTransient(sp =>
            {
                var connString = connectionString;
                return new DBConnection(connString);
            });

            // Register all validators from the assembly containing All Validators available
            services.AddValidatorsFromAssembly(typeof(AuthenticationCommandValidator).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>));

            // Since repositories and services are in different assemblies, we need to register them separately
            // and we can use assembly scanning to register all handlers in the application layer
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(
                    Assembly.GetExecutingAssembly(),
                    typeof(GetUserWantsToAuthenticateHandler).Assembly
                );
            });

            // UnitOfWork is registered as scoped because it manages the lifetime of the database connection and transaction,
            // which should be shared within a single request but not across multiple requests.
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register repositories and services
            services.AddTokenGeneratorFactory(configuration);
            services.AddTokenValidatorFactory(configuration);
            services.AddProductRepository(configuration);
            services.AddAuthenticationFeatures(configuration);
            services.AddAdminRepositories(configuration);

            return services;
        }

        public static IServiceCollection AddTokenGeneratorFactory(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
        }

        public static IServiceCollection AddTokenValidatorFactory(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped<ITokenValidator, JwtTokenValidator>();
        }

        public static IServiceCollection AddAuthenticationFeatures(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped<IUserAuthenticationRepository, UserAuthenticationRepository>();
        }

        public static IServiceCollection AddProductRepository(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped<IProductRepository, ProductRepository>();
        }

        public static IServiceCollection AddAdminRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddScoped<IAdminUserRepository, AdminUserRepository>();
        }
    }
}