using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data;
using System.Data;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        protected readonly DBConnection dbconn = new DBConnection();
        public async Task<List<Product>> GetAllProductAsync()
        {
            DataTable dt = await dbconn.ExecuteToDataTableAsync(
            "[Administration].[SP_GetAllProducts]",
            null,
            CommandType.StoredProcedure
            );

            return dbconn.ConvertDataTableToList<Product>(dt);
        }

        public async Task<bool> RemoveProductAsync(Product product)
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("ProductId", product.Id);

            var result = await dbconn.ExecuteCommandAsync("[Administration].[SP_RemoveProduct]", dictionary, CommandType.StoredProcedure);
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

            var result = await dbconn.ExecuteScalarAsync("[Administration].[SP_UpdateProduct]", dictionary, CommandType.StoredProcedure);

            return result;
        }
    }
}
