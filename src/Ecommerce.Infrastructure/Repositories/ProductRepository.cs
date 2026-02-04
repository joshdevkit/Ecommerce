using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Ecommerce.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public async Task<List<Product>> GetAllProductAsync()
        {
            DBConnection dbconn = new DBConnection();

            DataTable dt = await dbconn.ExecuteToDataTableAsync(
                    "[Administration].[SP_GetAllProducts]",
                    null,
                    CommandType.StoredProcedure
                );

            return dbconn.ConvertDataTableToList<Product>(dt);
        }
    }
}
