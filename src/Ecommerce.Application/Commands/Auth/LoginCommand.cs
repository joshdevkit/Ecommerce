using Ecommerce.Domain.Common;
using Ecommerce.Application.DTOs;
using MediatR;

namespace Ecommerce.Application.Commands.Auth
{
    public class LoginCommand : IRequest<Result<(UserDto User, string Token)>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
