using FluentValidation;

namespace Application.Features.Users.GetUserJobInfo;

/// <summary>
/// Validator for the <see cref="GetUserJobInfoQuery"/>.
/// </summary>
public sealed class GetUserJobInfoValidator : AbstractValidator<GetUserJobInfoQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserJobInfoValidator"/> class.
    /// </summary>
    public GetUserJobInfoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("User Id must be greater than zero.");
    }
}

