using Ecommerce.Application.DTOs;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Interfaces.Auth;
using Ecommerce.Domain.Common;
using MediatR;

namespace Ecommerce.Application.Commands.Auth
{
    public class GetUserWantsToAuthenticateHandler(
        IUserAuthenticationRepository repository,
        ITokenGenerator tokenGenerator,
        IUnitOfWork unitOfWork) : IRequestHandler<LoginCommand, Result<(UserDto User, string Token)>>
    {
        private readonly IUserAuthenticationRepository _repository = repository;
        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<(UserDto User, string Token)>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var user = await _repository.LoginAsync(request.Email, request.Password);

                if (user == null)
                {
                    _unitOfWork.Rollback();
                    return Result<(UserDto, string)>.Fail("Invalid credentials provided");
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

                var token = _tokenGenerator.GenerateToken(user);

                _unitOfWork.Commit();

                return Result<(UserDto, string)>.Ok((userDto, token), "Login Successful");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return Result<(UserDto, string)>.Fail($"An error occurred during login: {ex.Message}");
            }
        }
    }

}
