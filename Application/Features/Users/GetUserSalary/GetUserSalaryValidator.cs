using FluentValidation;

namespace Application.Features.Users.GetUserSalary;

public sealed class GetUserSalaryValidator : AbstractValidator<GetUserSalaryQuery>
{
    public GetUserSalaryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("User Id must be greater than zero.");
    }
}

