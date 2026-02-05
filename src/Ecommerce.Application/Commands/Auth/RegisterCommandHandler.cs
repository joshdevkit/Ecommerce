using Ecommerce.Application.Interfaces.Auth;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Commands.Auth
{
    public class RegisterCommandHandler(IUserAuthenticationRepository repository) : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly IUserAuthenticationRepository _repository = repository;

        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _repository.EmailExistsAsync(request.Email);
            if (emailExists)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = "Email already registered"
                };
            }

            var user = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _repository.RegisterAsync(user, request.Password);

            if (result == "Registration successful")
            {
                return new RegisterResponse
                {
                    Success = true,
                    Message = result
                };
            }

            return new RegisterResponse
            {
                Success = false,
                Message = result
            };
        }
    }
}
