using Ecommerce.Application.Interfaces.Admin;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data;
using System.Data;

namespace Ecommerce.Infrastructure.Repositories.Admin
{
    public class AdminUserRepository(DBConnection db) : IAdminUserRepository
    {
        private readonly DBConnection _db = db;
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            DataTable dt = await _db.ExecuteToDataTableAsync(
           "[Administration].[Users_GetAllActiveUsers]",
           null,
           CommandType.StoredProcedure
           );

            var response = _db.ConvertDataTableToList<User>(dt);

            return response;
        }
        public Task<bool> DeleteUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetUserByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
