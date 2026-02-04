using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Interfaces.Auth;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Repositories;
using Ecommerce.Infrastructure.Repositories.Auth;
using Ecommerce.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Ecommerce.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection")
                                     ?? throw new ArgumentNullException("DefaultConnection missing in appsettings.json");

            services.AddTransient(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connString = configuration.GetConnectionString("DefaultConnection");
                return new DBConnection(connString);
            });

            services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUserAuthenticationRepository, UserAuthenticationRepository>();

            return services;
        }
    }
}
