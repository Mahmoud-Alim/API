using Application.Common.Constants;
using FluentValidation;

namespace Application.Features.Roles.GetUserRoles;

public sealed class GetUserRolesValidator : AbstractValidator<GetUserRolesQuery>
{
    public GetUserRolesValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage(ValidationConstants.UserIdRequiredMessage);
    }
}
