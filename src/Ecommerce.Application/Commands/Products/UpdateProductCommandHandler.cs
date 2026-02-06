using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Common;
using Ecommerce.Domain.Entities;
using MediatR;

namespace Ecommerce.Application.Commands.Products
{
    public class UpdateProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateProductCommand, Result<Product>>
    {
        private readonly IProductRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Product>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var product = new Product
                {
                    Id = request.Id,
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Stock = request.Stock
                };

                var updateResult = await _repository.UpdateProductAsync(product);

                _unitOfWork.Commit();

                if (updateResult == "Success")
                    return Result<Product>.Ok(product);

                return Result<Product>.Fail(updateResult);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();

                return Result<Product>.Fail($"An error occurred while updating the product: {ex.Message}");
            }
        }
    }
}
