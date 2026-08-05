using Application.Common.Constants;
using FluentValidation;

namespace Application.Features.Users.GetAllUsers;

public sealed class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
{
    public GetAllUsersQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0)
            .WithMessage(ValidationConstants.PageNumberGreaterThanZeroMessage);

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage(ValidationConstants.PageSizeGreaterThanZeroMessage)
            .LessThanOrEqualTo(ValidationConstants.PageSizeMaxValue)
            .WithMessage(ValidationConstants.PageSizeMaxMessage);

        RuleFor(x => x.SortBy)
            .Must(sortBy => sortBy is null ||
                           ValidationConstants.AllowedSortFields.Contains(
                               sortBy, StringComparer.OrdinalIgnoreCase))
            .WithMessage(ValidationConstants.SortByAllowedMessage);

        When(x => !string.IsNullOrWhiteSpace(x.SearchTerm), () =>
        {
            RuleFor(x => x.SearchTerm!)
                .MaximumLength(ValidationConstants.SearchTermMaxLength)
                .WithMessage(ValidationConstants.SearchTermMaxLengthMessage);
        });

        When(x => !string.IsNullOrWhiteSpace(x.Gender), () =>
        {
            RuleFor(x => x.Gender!)
                .Must(gender => ValidationConstants.AllowedGenders.Contains(
                    gender, StringComparer.OrdinalIgnoreCase))
                .WithMessage(ValidationConstants.GenderAllowedMessage);
        });
    }
}
