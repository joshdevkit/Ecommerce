
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductRaw>> GetAllProductAsync();

        Task<string> UpdateProductAsync(Product product);

        Task<bool> RemoveProductAsync(Product product);
    }
}
