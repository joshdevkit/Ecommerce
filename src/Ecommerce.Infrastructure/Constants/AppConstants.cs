namespace Ecommerce.Infrastructure.Constants
{
    public static class AppConstants
    {
        public static class Jwt
        {
            public const string Key = "th!s !is v3ry s3cr3t k3y into my 3ntir3 l!f3";
            public const string Issuer = "Ecommerce.Api";
            public const string Audience = "Ecommerce.Client";
            public const int ExpiryHours = 2;
        }
    }
}
