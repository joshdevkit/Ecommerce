using Ecommerce.Application.Commands.Auth;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Commands.Products
{
    public class UpdateProductCommandHandler(IProductRepository repository) : IRequestHandler<UpdateProductCommand, UpdateResponse>
    {
        private readonly IProductRepository _repository = repository;

        public async Task<UpdateResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var payload = new Product
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock
            };

            var result = await _repository.UpdateProductAsync(payload);


            if (result == "Success")
            {
                return new UpdateResponse
                {
                    Success = true,
                    Message = result.ToString()
                };
            }

            return new UpdateResponse
            {
                Success = false,
                Message = result.ToString()
            };
        }

    }
}
