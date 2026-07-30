using Application.Common.Models;
using MediatR;

namespace Application.Features.Users.UserExists;

/// <summary>
/// Query to check whether a user exists by their unique identifier.
/// </summary>
public sealed record UserExistsQuery : IRequest<Result<bool>>
{
    /// <summary>
    /// Gets the unique identifier of the user to check.
    /// </summary>
    public int Id { get; init; }
}

