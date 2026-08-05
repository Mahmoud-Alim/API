using Application.Common.Constants;
using FluentValidation;

namespace Application.Features.Auth.RefreshTokenFeature;

public sealed class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty().WithMessage(ValidationConstants.AccessTokenRequiredMessage);
    }
}
