using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces.Admin;
using MediatR;

namespace Ecommerce.Application.Queries.Admin
{
    public class GetAllUsersQueryForAdminHandler(IAdminUserRepository userRepository) : IRequestHandler<GetAllUsersQueryForAdmin, List<UserDto>>
    {
        private readonly IAdminUserRepository _userRepository = userRepository;
        public async Task<List<UserDto>> Handle(GetAllUsersQueryForAdmin request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllUsersAsync();
            var userDtos = users.Select(user => new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                Role = user.Role
            }).ToList();
            return userDtos;
        }
    }
}
