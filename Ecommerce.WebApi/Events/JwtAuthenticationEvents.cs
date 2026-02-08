using Ecommerce.Infrastructure.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json;

namespace Ecommerce.WebApi.Events
{
    public class JwtAuthenticationEvents : JwtBearerEvents
    {
        public override Task MessageReceived(MessageReceivedContext context)
        {
            if (context.Request.Cookies.TryGetValue(AppConstants.Jwt.CookieName, out var token))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }
        public override Task Challenge(JwtBearerChallengeContext context)
        {
            context.HandleResponse();

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            var response = JsonSerializer.Serialize(new
            {

                success = false,
                message = "[Unauthorized] Authentication Error"
            });

            return context.Response.WriteAsync(response);
        }

        public override Task Forbidden(ForbiddenContext context)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            var response = JsonSerializer.Serialize(new
            {
                success = false,
                message = "[Forbidden] Authorization Error"
            });

            return context.Response.WriteAsync(response);
        }
    }
}
