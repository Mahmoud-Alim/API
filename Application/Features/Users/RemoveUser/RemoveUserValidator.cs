using FluentValidation;

namespace Application.Features.Users.RemoveUser;

/// <summary>
/// Validator for the <see cref="RemoveUserCommand"/>.
/// </summary>
public sealed class RemoveUserValidator : AbstractValidator<RemoveUserCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveUserValidator"/> class.
    /// </summary>
    public RemoveUserValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("User Id must be greater than zero.");
    }
}

