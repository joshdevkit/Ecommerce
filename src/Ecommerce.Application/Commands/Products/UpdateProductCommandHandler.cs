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
    public class UpdateProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductCommand, UpdateResponse>
    {
        private readonly IProductRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UpdateResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var payload = new Product
                {
                    Id = request.Id,
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Stock = request.Stock
                };

                var result = await _repository.UpdateProductAsync(payload);

                _unitOfWork.Commit();

                return new UpdateResponse
                {
                    Success = result == "Success",
                    Message = result
                };

            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();

                return new UpdateResponse
                {
                    Success = false,
                    Message = $"An error occurred while updating the product: {ex.Message}"
                };

            }
        }

    }
}
