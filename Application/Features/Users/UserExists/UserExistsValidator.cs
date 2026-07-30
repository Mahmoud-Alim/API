using FluentValidation;

namespace Application.Features.Users.UserExists;

/// <summary>
/// Validator for the <see cref="UserExistsQuery"/>.
/// </summary>
public sealed class UserExistsValidator : AbstractValidator<UserExistsQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserExistsValidator"/> class.
    /// </summary>
    public UserExistsValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("User Id must be greater than zero.");
    }
}

