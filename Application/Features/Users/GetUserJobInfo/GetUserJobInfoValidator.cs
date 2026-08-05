using Application.Common.Constants;
using FluentValidation;

namespace Application.Features.Users.GetUserJobInfo;

public sealed class GetUserJobInfoValidator : AbstractValidator<GetUserJobInfoQuery>
{
    public GetUserJobInfoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(ValidationConstants.UserIdGreaterThanZeroMessage);
    }
}

