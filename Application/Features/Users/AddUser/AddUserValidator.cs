using Application.Common.Constants;
using FluentValidation;

namespace Application.Features.Users.AddUser;

public sealed class AddUserValidator : AbstractValidator<AddUserCommand>
{
    public AddUserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(ValidationConstants.FirstNameRequiredMessage)
            .MinimumLength(ValidationConstants.NameMinLength)
            .MaximumLength(ValidationConstants.NameMaxLength)
            .WithMessage(ValidationConstants.FirstNameLengthRangeMessage);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(ValidationConstants.LastNameRequiredMessage)
            .MinimumLength(ValidationConstants.NameMinLength)
            .MaximumLength(ValidationConstants.NameMaxLength)
            .WithMessage(ValidationConstants.LastNameLengthRangeMessage);

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
            .MinimumLength(ValidationConstants.GenderMinLength)
            .MaximumLength(ValidationConstants.GenderMaxLength)
            .WithMessage(ValidationConstants.GenderLengthRangeMessage);
    }
}

