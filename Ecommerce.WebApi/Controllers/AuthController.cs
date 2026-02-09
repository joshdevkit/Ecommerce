using Ecommerce.Application.Commands.Auth;
using Ecommerce.Application.Queries.Auth;
using Ecommerce.Infrastructure.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator, IWebHostEnvironment env) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IWebHostEnvironment _env = env;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);

            var (user, token) = result.Value!;
            Response.Cookies.Append(
                AppConstants.Jwt.CookieName,
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = !_env.IsDevelopment(),
                    SameSite = _env.IsProduction()
                        ? SameSiteMode.Strict
                        : SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddMinutes(
                        AppConstants.Jwt.ExpiryMinutes
                    ),
                    Path = "/" 
                }
            );

            return Ok(new
            {
                success = true,
                message = result.Message,
                user
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success) return BadRequest(result);

            var user = result.Value!;

            return Ok(new
            {
                Success = true,
                result.Message,
                User = user
            });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> UserAuth()
        {
            var emailClaim = User.Claims.FirstOrDefault(static c => c.Type == ClaimTypes.Email);
            if (emailClaim is null || string.IsNullOrEmpty(emailClaim.Value))
            {
                return Unauthorized(new { Success = false, Message = "User not found." });
            }

            var user = await _mediator.Send(new GetUserByEmailQuery
            {
                Email = emailClaim.Value
            });

            return Ok(new
            {
                Success = true,
                user
            });
        }


        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(AppConstants.Jwt.CookieName);

            return Ok(new
            {
                success = true,
                message = "Logged out successfully"
            });
        }
    }
}
