using Ecommerce.Application.Commands.Auth;
using FluentValidation;

namespace Ecommerce.Application.Validators
{
    public class AuthenticationCommandValidator : AbstractValidator<RegisterCommand>
    {
        public AuthenticationCommandValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");
            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters");
        }
    }
}
