using Ecommerce.Application.Interfaces.Auth;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data;
using System.Data;

namespace Ecommerce.Infrastructure.Repositories.Auth
{
    public class UserAuthenticationRepository(DBConnection db) : IUserAuthenticationRepository
    {
        private readonly DBConnection _db = db;
        public async Task<User?> LoginAsync(string email, string password)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@Email", email);
            parameters.Add("@Password", password);

            DataTable dt = await _db.ExecuteToDataTableAsync(
                "[Authentication].[SP_UserLogin]",
                parameters,
                CommandType.StoredProcedure
            );

            var users = _db.ConvertDataTableToList<User>(dt);
            return users.FirstOrDefault();
        }

        public async Task<string> RegisterAsync(User user, string password)
        {

            var parameters = new Dictionary<string, object>();
            parameters.Add("@Email", user.Email);
            parameters.Add("@Password", password);
            parameters.Add("@FirstName", user.FirstName);
            parameters.Add("@LastName", user.LastName);
            parameters.Add("@PhoneNumber", user.PhoneNumber ?? (object)DBNull.Value);
          

            var result = await _db.ExecuteScalarAsync(
                "[Authentication].[SP_UserRegistration]",
                parameters,
                CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {

            var parameters = new Dictionary<string, object>();
            parameters.Add("@Email", email);

            DataTable dt = await _db.ExecuteToDataTableAsync(
            "[Authentication].[SP_GetUserByEmail]",
            parameters,
            CommandType.StoredProcedure
            );

            var users = _db.ConvertDataTableToList<User>(dt);
            return users.FirstOrDefault();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@Email", email);

            string result = await _db.ExecuteScalarAsync(
                "[Authentication].[SP_CheckEmailExists]",
                parameters,
                CommandType.StoredProcedure
            );

            return result == "1" || result.Equals("true", StringComparison.CurrentCultureIgnoreCase);
        }

        public async Task<User> UpdateUserProfile(User User)
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("@UserId", User.UserId);
            dictionary.Add("@FirstName", User.FirstName);
            dictionary.Add("@LastName", User.LastName);
            dictionary.Add("@PhoneNumber", User.PhoneNumber ?? (object)DBNull.Value);

            var response = await _db.ExecuteToDataTableAsync(
                "[Authentication].[SP_UpdateUserProfile]",
                dictionary,
                CommandType.StoredProcedure
            );

            var users = _db.ConvertDataTableToList<User>(response);
            return users.First();
        }
    }
}
