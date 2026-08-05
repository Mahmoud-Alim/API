using Application.Common.Constants;
using FluentValidation;

namespace Application.Features.Auth.RevokeToken;

public sealed class RevokeTokenValidator : AbstractValidator<RevokeTokenCommand>
{
    public RevokeTokenValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage(ValidationConstants.TokenRequiredMessage);
    }
}
