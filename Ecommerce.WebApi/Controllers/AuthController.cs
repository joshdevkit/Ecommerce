using Ecommerce.Application.Commands.Auth;
using Ecommerce.Application.Queries.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);

            var (user, token) = result.Value!;
            return Ok(new
            {
                Success = true,
                result.Message,
                User = user,
                Token = token
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
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var user = await _mediator.Send(new GetUserByEmailQuery
            {
                Email = email
            });

            return Ok(new
            {
                Success = true,
                user
            });
        }

    }
}
