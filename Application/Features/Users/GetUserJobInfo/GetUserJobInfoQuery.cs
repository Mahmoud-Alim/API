using Application.Common.Models;
using MediatR;

namespace Application.Features.Users.GetUserJobInfo;

/// <summary>
/// Query to retrieve a user's job information.
/// </summary>
public sealed record GetUserJobInfoQuery : IRequest<Result<UserJobInfoDto>>
{
    /// <summary>
    /// Gets the unique identifier of the user.
    /// </summary>
    public int Id { get; init; }
}

