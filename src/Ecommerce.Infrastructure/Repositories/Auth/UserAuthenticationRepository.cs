using Ecommerce.Application.Interfaces.Auth;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data;
using System.Data;

namespace Ecommerce.Infrastructure.Repositories.Auth
{
    public class UserAuthenticationRepository : IUserAuthenticationRepository
    {
        protected readonly DBConnection dbconn = new DBConnection();
        public async Task<User?> LoginAsync(string email, string password)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@Email", email);
            parameters.Add("@Password", password);

            DataTable dt = await dbconn.ExecuteToDataTableAsync(
                "[Authentication].[SP_UserLogin]",
                parameters,
                CommandType.StoredProcedure
            );

            var users = dbconn.ConvertDataTableToList<User>(dt);
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
          

            var result = await dbconn.ExecuteScalarAsync(
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

            DataTable dt = await dbconn.ExecuteToDataTableAsync(
            "[Authentication].[SP_GetUserByEmail]",
            parameters,
            CommandType.StoredProcedure
            );

            var users = dbconn.ConvertDataTableToList<User>(dt);
            return users.FirstOrDefault();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("@Email", email);

            string result = await dbconn.ExecuteScalarAsync(
                "[Authentication].[SP_CheckEmailExists]",
                parameters,
                CommandType.StoredProcedure
            );

            return result == "1" || result.Equals("true", StringComparison.CurrentCultureIgnoreCase);
        }

        public Task<User> UpdateUserProfile(User User)
        {
            throw new NotImplementedException();
        }
    }
}
