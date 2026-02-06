using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces.Auth;
using MediatR;

namespace Ecommerce.Application.Commands.Auth
{
    public class GetUserWantsToAuthenticateHandler(
        IUserAuthenticationRepository repository,
        ITokenGenerator tokenGenerator) : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserAuthenticationRepository _repository = repository;
        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.LoginAsync(request.Email, request.Password);

            if (user == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Invalid crendentials provided"
                };
            }

            return new LoginResponse
            {
                Success = true,
                Message = "Login successful",
                User = new UserDto
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role
                },
                Token = _tokenGenerator.GenerateToken(user)
            };
        }
    }
}
