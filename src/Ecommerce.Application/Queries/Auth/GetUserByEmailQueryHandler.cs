using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces.Auth;
using MediatR;
namespace Ecommerce.Application.Queries.Auth
{
    public class GetUserByEmailQueryHandler(IUserAuthenticationRepository repository) : IRequestHandler<GetUserByEmailQuery, UserDto?>
    {
        private readonly IUserAuthenticationRepository _repository = repository;

        public async Task<UserDto?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetUserByEmailAsync(request.Email);

            if (user == null)
                return null;

            return new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role
            };
        }
    }
}
