using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data;
using System.Data;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ProductRepository(DBConnection db) : IProductRepository
    {
        private readonly DBConnection _db = db;

        public async Task<List<ProductRaw>> GetAllProductAsync()
        {
            DataTable dt = await _db.ExecuteToDataTableAsync(
            "[Administration].[SP_GetAllProducts]",
            null,
            CommandType.StoredProcedure
            );

            var response = _db.ConvertDataTableToList<ProductRaw>(dt);

            return response;
        }

        public async Task<bool> RemoveProductAsync(Product product)
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("ProductId", product.Id);

            var result = await _db.ExecuteCommandAsync("[Administration].[SP_RemoveProduct]", dictionary, CommandType.StoredProcedure);
            return result > 0;
        }

        public async Task<string> UpdateProductAsync(Product product)
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("ProductId", product.Id);
            dictionary.Add("Name", product.Name);
            dictionary.Add("Description", product.Description);
            dictionary.Add("Price", product.Price);
            dictionary.Add("Stock", product.Stock);

            var result = await _db.ExecuteScalarAsync("[Administration].[SP_UpdateProduct]", dictionary, CommandType.StoredProcedure);

            return result;
        }
    }
}
