using FluentValidation;

namespace Application.Features.Roles.GetUserRoles;

public sealed class GetUserRolesValidator : AbstractValidator<GetUserRolesQuery>
{
    public GetUserRolesValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}