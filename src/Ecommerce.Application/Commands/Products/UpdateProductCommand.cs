using Ecommerce.Domain.Common;
using Ecommerce.Domain.Entities;
using MediatR;
using System;

namespace Ecommerce.Application.Commands.Products
{
    public class UpdateProductCommand : IRequest<Result<Product>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
