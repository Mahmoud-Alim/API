using Application.Common.Models;
using MediatR;

namespace Application.Features.Users.GetUserJobInfo;

public sealed record GetUserJobInfoQuery : IRequest<Result<UserJobInfoDto>>
{
    public int Id { get; init; }
}

