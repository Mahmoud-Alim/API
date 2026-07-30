using FluentValidation;

namespace Application.Features.Users.GetActiveUsers;

/// <summary>
/// Validator for the <see cref="GetActiveUsersQuery"/>.
/// Since the query has no parameters, this validator performs no specific validations.
/// </summary>
public sealed class GetActiveUsersValidator : AbstractValidator<GetActiveUsersQuery>
{
}

