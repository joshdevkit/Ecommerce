using Ecommerce.Application.Commands.Products;
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

        [HttpPut]
        public async Task<ActionResult<Product>> Update([FromBody] UpdateProductCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
