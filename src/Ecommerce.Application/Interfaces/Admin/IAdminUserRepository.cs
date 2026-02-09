
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Admin
{
    public interface IAdminUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();

        Task<User?> GetUserByIdAsync(int userId);

        Task<bool> UpdateUserAsync(User user);

        Task<bool> DeleteUserAsync(int userId);
    }
}
