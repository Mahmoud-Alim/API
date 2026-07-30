using FluentValidation;

namespace Application.Features.Roles.AddRole;

public sealed class AddRoleValidator : AbstractValidator<AddRoleCommand>
{
    public AddRoleValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage("Role name is required.")
            .MinimumLength(3).WithMessage("Role name must be at least 3 characters.")
            .MaximumLength(50).WithMessage("Role name must not exceed 50 characters.");
    }
}