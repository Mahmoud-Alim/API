using FluentValidation;

namespace Application.Features.Users.RemoveUser;

public sealed class RemoveUserValidator : AbstractValidator<RemoveUserCommand>
{
    public RemoveUserValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("User Id must be greater than zero.");
    }
}

