using System.Security.Claims;

namespace Ecommerce.Application.Interfaces.Auth
{
    public interface ITokenValidator
    {
        ClaimsPrincipal? ValidateToken(string token);

        bool IsTokenValid(string token);
    }
}