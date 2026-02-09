using Ecommerce.Application.Interfaces.Auth;
using Ecommerce.Infrastructure.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Infrastructure.Services
{
    public class JwtTokenValidator : ITokenValidator
    {
        private readonly TokenValidationParameters _validationParameters;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public JwtTokenValidator(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            // Create signing key for signature verification
            var signingKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(AppConstants.Jwt.Key)
            );

            // Create encryption key for token decryption
            var encryptionKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(AppConstants.Jwt.EncryptionKey)
            );

            // Configure token validation parameters
            _validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = AppConstants.Jwt.Issuer,
                ValidAudience = AppConstants.Jwt.Audience,
                IssuerSigningKey = signingKey,
                TokenDecryptionKey = encryptionKey, // Critical for JWE decryption
                ClockSkew = TimeSpan.Zero // No tolerance for expired tokens

            };

            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            try
            {
                // Validate and decrypt the token
                var principal = _tokenHandler.ValidateToken(
                    token,
                    _validationParameters,
                    out SecurityToken validatedToken
                );

                // Ensure the token is a valid JwtSecurityToken
                if (validatedToken is not JwtSecurityToken jwtToken)
                    return null;

                // Additional validation: ensure token is encrypted (JWE format)
                // JWE tokens have 5 parts, JWS tokens have 3 parts
                var tokenParts = token.Split('.');
                if (tokenParts.Length != 5)
                {
                    return null;
                }

                return principal;
            }
            catch (SecurityTokenExpiredException)
            {
                // Token has expired
                return null;
            }
            catch (SecurityTokenInvalidSignatureException)
            {
                // Token signature is invalid
                return null;
            }
            catch (SecurityTokenDecryptionFailedException)
            {
                // Token decryption failed
                return null;
            }
            catch (Exception)
            {
                // Any other validation error
                return null;
            }
        }
        public bool IsTokenValid(string token)
        {
            return ValidateToken(token) != null;
        }
    }
}