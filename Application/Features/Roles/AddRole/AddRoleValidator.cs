using Application.Common.Constants;
using FluentValidation;

namespace Application.Features.Roles.AddRole;

public sealed class AddRoleValidator : AbstractValidator<AddRoleCommand>
{
    public AddRoleValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage(ValidationConstants.RoleNameRequiredMessage)
            .MinimumLength(ValidationConstants.RoleNameMinLength).WithMessage(ValidationConstants.RoleNameMinLengthMessage)
            .MaximumLength(ValidationConstants.RoleNameMaxLength).WithMessage(ValidationConstants.RoleNameMaxLengthMessage);
    }
}
