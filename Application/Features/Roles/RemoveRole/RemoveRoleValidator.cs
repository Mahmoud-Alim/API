using Application.Common.Constants;
using FluentValidation;

namespace Application.Features.Roles.RemoveRole;

public sealed class RemoveRoleValidator : AbstractValidator<RemoveRoleCommand>
{
    public RemoveRoleValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty().WithMessage(ValidationConstants.RoleNameRequiredMessage);
    }
}
