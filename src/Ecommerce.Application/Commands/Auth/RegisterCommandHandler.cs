using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Interfaces.Auth;
using Ecommerce.Domain.Common;
using Ecommerce.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Application.Commands.Auth
{
    public class RegisterCommandHandler(
        IUserAuthenticationRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<RegisterCommand, Result<UserDto>>
    {
        private readonly IUserAuthenticationRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<UserDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var emailExists = await _repository.EmailExistsAsync(request.Email);
                if (emailExists)
                {
                    _unitOfWork.Rollback();
                    return Result<UserDto>.Fail("Email already registered");
                }

                var user = new User
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    Role = "User" // default role
                };

                var registrationResult = await _repository.RegisterAsync(user, request.Password);

                if (registrationResult != "Registration successful")
                {
                    _unitOfWork.Rollback();
                    return Result<UserDto>.Fail(registrationResult);
                }

                var userDto = new UserDto
                {
                    UserId = user.UserId,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role
                };

                _unitOfWork.Commit();

                return Result<UserDto>.Ok(userDto, "Registration successful");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return Result<UserDto>.Fail($"An error occurred during registration: {ex.Message}");
            }
        }
    }
}
