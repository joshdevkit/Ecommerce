using Ecommerce.Application.Queries.Product;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IMediator _mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Product>>> Index()
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }
    }
}
