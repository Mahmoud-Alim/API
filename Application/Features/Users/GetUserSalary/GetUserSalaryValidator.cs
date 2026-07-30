using FluentValidation;

namespace Application.Features.Users.GetUserSalary;

/// <summary>
/// Validator for the <see cref="GetUserSalaryQuery"/>.
/// </summary>
public sealed class GetUserSalaryValidator : AbstractValidator<GetUserSalaryQuery>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserSalaryValidator"/> class.
    /// </summary>
    public GetUserSalaryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("User Id must be greater than zero.");
    }
}

