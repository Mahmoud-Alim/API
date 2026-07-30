using FluentValidation;

namespace Application.Features.Roles.PromoteUser;

public sealed class PromoteUserValidator : AbstractValidator<PromoteUserCommand>
{
    public PromoteUserValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}