using Application.Common.Constants;
using FluentValidation;

namespace Application.Features.Auth.Login;

public sealed class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationConstants.EmailRequiredMessage)
            .EmailAddress().WithMessage(ValidationConstants.EmailValidMessage);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationConstants.PasswordRequiredMessage);
    }
}
