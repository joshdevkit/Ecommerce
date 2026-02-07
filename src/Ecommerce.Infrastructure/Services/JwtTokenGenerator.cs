using Ecommerce.Application.Interfaces.Auth;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Infrastructure.Services
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        public string GenerateToken(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            // Define claims to be included in the token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            // Create signing credentials for token integrity (HMAC-SHA256)
            var signingKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(AppConstants.Jwt.Key)
            );
            var signingCredentials = new SigningCredentials(
                signingKey,
                SecurityAlgorithms.HmacSha256
            );

            // Create encryption credentials for token confidentiality (AES-256)
            var encryptionKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(AppConstants.Jwt.EncryptionKey)
            );
            var encryptingCredentials = new EncryptingCredentials(
                encryptionKey,
                SecurityAlgorithms.Aes256KW,           // Key Encryption Algorithm (AES Key Wrap)
                SecurityAlgorithms.Aes256CbcHmacSha512 // Content Encryption Algorithm
            );

            // Get expiry from configuration
            var expiryMinutes = AppConstants.Jwt.ExpiryMinutes;

            // Create token descriptor with both signing and encryption
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = AppConstants.Jwt.Issuer,
                Audience = AppConstants.Jwt.Audience,
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                NotBefore = DateTime.UtcNow, // Token valid immediately
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = signingCredentials,      // Sign for integrity
                EncryptingCredentials = encryptingCredentials // Encrypt for confidentiality
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Write token as JWE string (5 parts separated by dots)
            return tokenHandler.WriteToken(token);
        }
    }
}