using Ecommerce.Application.DTOs;
using MediatR;

namespace Ecommerce.Application.Queries.Product
{
    public class GetAllProductsQuery : IRequest<List<ProductDto>>;
}
