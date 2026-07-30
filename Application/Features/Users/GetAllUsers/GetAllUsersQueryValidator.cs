using FluentValidation;

namespace Application.Features.Users.GetAllUsers;

public sealed class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
{
    public GetAllUsersQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than zero.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be greater than zero.")
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must not exceed 100.");

        RuleFor(x => x.SortBy)
            .Must(sortBy => sortBy is null ||
                           sortBy.Equals("UserId", StringComparison.OrdinalIgnoreCase) ||
                           sortBy.Equals("FirstName", StringComparison.OrdinalIgnoreCase) ||
                           sortBy.Equals("LastName", StringComparison.OrdinalIgnoreCase) ||
                           sortBy.Equals("Email", StringComparison.OrdinalIgnoreCase) ||
                           sortBy.Equals("Gender", StringComparison.OrdinalIgnoreCase) ||
                           sortBy.Equals("Active", StringComparison.OrdinalIgnoreCase))
            .WithMessage("SortBy must be one of: UserId, FirstName, LastName, Email, Gender, Active.");

        // --- Filter validation ---
        When(x => !string.IsNullOrWhiteSpace(x.SearchTerm), () =>
        {
            RuleFor(x => x.SearchTerm!)
                .MaximumLength(100)
                .WithMessage("Search term must not exceed 100 characters.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Gender), () =>
        {
            RuleFor(x => x.Gender!)
                .Must(gender => gender.Equals("Male", StringComparison.OrdinalIgnoreCase) ||
                                gender.Equals("Female", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Gender must be either 'Male' or 'Female'.");
        });
    }
}
