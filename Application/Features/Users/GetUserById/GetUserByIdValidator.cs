using Application.Common.Constants;
using FluentValidation;

namespace Application.Features.Users.GetUserById;

public sealed class GetUserByIdValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(ValidationConstants.UserIdGreaterThanZeroMessage);
    }
}

