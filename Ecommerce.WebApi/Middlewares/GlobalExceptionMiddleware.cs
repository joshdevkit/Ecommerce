using FluentValidation;
using System.Text.Json;

namespace Ecommerce.WebApi.Middlewares
{
    public class GlobalExceptionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(x => x.ErrorMessage).ToArray()
                    );

                var response = new
                {
                    success = false,
                    message = "Registration Failed, please check all your inputs.",
                    errors
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response)
                );
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    success = false,
                    message = "An unexpected error occurred"
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response)
                );
            }
        }
    }
}
