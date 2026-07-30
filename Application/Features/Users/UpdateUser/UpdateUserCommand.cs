using Application.Common.Models;
using MediatR;

namespace Application.Features.Users.UpdateUser;

/// <summary>
/// Command to update an existing user.
/// </summary>
public sealed record UpdateUserCommand : IRequest<Result<UserDto>>
{
    /// <summary>
    /// Gets the unique identifier of the user to update.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the first name of the user.
    /// </summary>
    public string FirstName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the last name of the user.
    /// </summary>
    public string LastName { get; init; } = string.Empty;

    /// <summary>
    /// Gets the email of the user.
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Gets the gender of the user.
    /// </summary>
    public string Gender { get; init; } = string.Empty;

    /// <summary>
    /// Gets a value indicating whether the user is active.
    /// </summary>
    public bool Active { get; init; }
}

