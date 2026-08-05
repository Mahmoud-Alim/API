using Application.Common.Constants;
using FluentValidation;

namespace Application.Features.Auth.Register;

public sealed class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(ValidationConstants.FirstNameRequiredMessage)
            .MinimumLength(ValidationConstants.NameMinLength).WithMessage(ValidationConstants.FirstNameMinLengthMessage)
            .MaximumLength(ValidationConstants.NameMaxLength).WithMessage(ValidationConstants.FirstNameMaxLengthMessage);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(ValidationConstants.LastNameRequiredMessage)
            .MinimumLength(ValidationConstants.NameMinLength).WithMessage(ValidationConstants.LastNameMinLengthMessage)
            .MaximumLength(ValidationConstants.NameMaxLength).WithMessage(ValidationConstants.LastNameMaxLengthMessage);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ValidationConstants.EmailRequiredMessage)
            .EmailAddress().WithMessage(ValidationConstants.EmailValidMessage)
            .MaximumLength(ValidationConstants.EmailMaxLength).WithMessage(ValidationConstants.EmailMaxLengthMessage);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationConstants.PasswordRequiredMessage)
            .MinimumLength(ValidationConstants.PasswordMinLength).WithMessage(ValidationConstants.PasswordMinLengthMessage)
            .MaximumLength(ValidationConstants.PasswordMaxLength).WithMessage(ValidationConstants.PasswordMaxLengthMessage);
    }
}
