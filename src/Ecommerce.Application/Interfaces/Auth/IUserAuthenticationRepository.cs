
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces.Auth
{
    public interface IUserAuthenticationRepository
    {
        Task<User?> LoginAsync(string email, string password);
        Task<string> RegisterAsync(User user, string password);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<string> UpdateUserProfile(User User);
    }
}
