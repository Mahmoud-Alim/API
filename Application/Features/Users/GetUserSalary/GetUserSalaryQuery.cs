using Application.Common.Models;
using MediatR;

namespace Application.Features.Users.GetUserSalary;

/// <summary>
/// Query to retrieve a user's salary information.
/// </summary>
public sealed record GetUserSalaryQuery : IRequest<Result<UserSalaryDto>>
{
    /// <summary>
    /// Gets the unique identifier of the user.
    /// </summary>
    public int Id { get; init; }
}

