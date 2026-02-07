using Microsoft.Extensions.Configuration;

namespace Ecommerce.Infrastructure.Constants
{
    public static class AppConstants
    {
        public static class Jwt
        {
            public static string Key { get; private set; } = string.Empty;

            public static string EncryptionKey { get; private set; } = string.Empty;

            public static string Issuer { get; private set; } = string.Empty;

            public static string Audience { get; private set; } = string.Empty;

            public static int ExpiryMinutes { get; private set; } = 60;

            public static void Initialize(IConfiguration configuration)
            {
                Key = configuration["Jwt:Key"]
                    ?? throw new InvalidOperationException("Jwt:Key is missing in configuration. Please add a signing key with minimum 32 characters.");

                EncryptionKey = configuration["Jwt:EncryptionKey"]
                    ?? throw new InvalidOperationException("Jwt:EncryptionKey is missing in configuration. Please add an encryption key with minimum 32 characters.");

                Issuer = configuration["Jwt:Issuer"]
                    ?? throw new InvalidOperationException("Jwt:Issuer is missing in configuration.");

                Audience = configuration["Jwt:Audience"]
                    ?? throw new InvalidOperationException("Jwt:Audience is missing in configuration.");

                if (int.TryParse(configuration["Jwt:ExpiryMinutes"], out int expiry))
                {
                    ExpiryMinutes = expiry;
                }

                if (Key.Length < 32)
                {
                    throw new InvalidOperationException("Jwt:Key must be at least 32 characters long for HMAC-SHA256 security.");
                }

                if (EncryptionKey.Length < 32)
                {
                    throw new InvalidOperationException("Jwt:EncryptionKey must be at least 32 characters long for AES-256 encryption.");
                }
            }
        }
    }
}