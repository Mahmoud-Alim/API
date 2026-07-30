using FluentValidation;

namespace Application.Features.Users.AddUser;

public sealed class AddUserValidator : AbstractValidator<AddUserCommand>
{
    public AddUserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MinimumLength(3)
            .MaximumLength(100)
            .WithMessage("First name must be between 3 and 100 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MinimumLength(3)
            .MaximumLength(100)
            .WithMessage("Last name must be between 3 and 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .MaximumLength(200)
            .WithMessage("Email must not exceed 200 characters.")
            .EmailAddress()
            .WithMessage("A valid email address is required.");

        RuleFor(x => x.Gender)
            .NotEmpty()
            .WithMessage("Gender is required.")
            .MinimumLength(4)
            .MaximumLength(20)
            .WithMessage("Gender must be between 4 and 20 characters.");
    }
}

