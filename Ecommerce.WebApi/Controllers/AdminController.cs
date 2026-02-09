using Ecommerce.Application.Queries.Admin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecommerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class AdminController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpGet("users")]
        public async Task<IActionResult> Users()
        {
            var response = await _mediator.Send(new GetAllUsersQueryForAdmin());
            return Ok(response);
        }
    }
}
