using Application.Common.Models;
using MediatR;

namespace Application.Features.Users.RemoveUser;

/// <summary>
/// Command to remove a user by their unique identifier.
/// </summary>
public sealed record RemoveUserCommand : IRequest<Result<bool>>
{
    /// <summary>
    /// Gets the unique identifier of the user to remove.
    /// </summary>
    public int Id { get; init; }
}

