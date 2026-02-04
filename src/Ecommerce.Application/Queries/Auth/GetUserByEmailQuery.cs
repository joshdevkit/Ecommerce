using Ecommerce.Application.DTOs;
using MediatR;

namespace Ecommerce.Application.Queries.Auth
{
    public class GetUserByEmailQuery : IRequest<UserDto?>
    {
        public string Email { get; set; } = string.Empty;
    }
}
