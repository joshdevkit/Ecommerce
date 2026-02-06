using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using MediatR;
using System.Linq;

namespace Ecommerce.Application.Queries.Product
{
    public class GetAllProductsQueryHandler(
        IProductRepository repository
    ) : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _repository = repository;

        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _repository.GetAllProductAsync();

            var productDtos = products.Select(product => new ProductDto
            {
                Id = product.Id,
                SKU = product.SKU,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                MetaTitle = product.MetaTitle,
                MetaDescription = product.MetaDescription,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                Stock = product.Stock,
                Weight = product.Weight,
                Status = product.Status,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,

                CategoryName = product.CategoryName,
                CategorySlug = product.CategorySlug,
                CategoryMetaTitle = product.CategoryMetaTitle,
                CategoryDescription = product.CategoryDescription,

                BrandName = product.BrandName,
                BrandSlug = product.BrandSlug,
                BrandMetaTitle = product.BrandMetaTitle,
                BrandMetaDescription = product.BrandMetaDescription,

                CreatedByUserName = product.CreatedByUserName

            }).ToList();

            return productDtos;
        }
    }
}
