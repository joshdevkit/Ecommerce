using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Common;
using MediatR;

namespace Ecommerce.Application.Commands.Auth
{
    public class RegisterCommand : IRequest<Result<UserDto>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordConfirmation { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }
}
