using Ecommerce.Application.DTOs;
using MediatR;

namespace Ecommerce.Application.Queries.Admin
{
    public class GetAllUsersQueryForAdmin : IRequest<List<UserDto>>;
}
