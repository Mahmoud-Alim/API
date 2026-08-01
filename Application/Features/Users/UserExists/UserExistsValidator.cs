using FluentValidation;

namespace Application.Features.Users.UserExists;

public sealed class UserExistsValidator : AbstractValidator<UserExistsQuery>
{
    public UserExistsValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("User Id must be greater than zero.");
    }
}

