using Ecommerce.Application.Commands.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Commands.Products
{
    public class UpdateProductCommand : IRequest<UpdateResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }

    public class UpdateResponse
    {
        public bool Success { get; set; }

        public string Message { get; set;} = string.Empty;
    }
}
