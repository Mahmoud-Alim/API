using Application.Common.Constants;
using FluentValidation;

namespace Application.Features.Users.UpdateUser;

public sealed class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(ValidationConstants.UserIdGreaterThanZeroMessage);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(ValidationConstants.FirstNameRequiredMessage)
            .MaximumLength(ValidationConstants.NameMaxLength)
            .WithMessage(ValidationConstants.FirstNameMaxLengthMessage);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(ValidationConstants.LastNameRequiredMessage)
            .MaximumLength(ValidationConstants.NameMaxLength)
            .WithMessage(ValidationConstants.LastNameMaxLengthMessage);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(ValidationConstants.EmailRequiredMessage)
            .MaximumLength(200)
            .WithMessage(ValidationConstants.EmailMaxLength200Message)
            .EmailAddress()
            .WithMessage(ValidationConstants.EmailValidMessage);

        RuleFor(x => x.Gender)
            .NotEmpty()
            .WithMessage(ValidationConstants.GenderRequiredMessage)
            .MaximumLength(ValidationConstants.GenderMaxLength)
            .WithMessage(ValidationConstants.GenderMaxLengthMessage);
    }
}

