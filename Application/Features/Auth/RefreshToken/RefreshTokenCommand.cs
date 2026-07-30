using Application.Common.Models;
using Application.Common.Models.Auth;
using MediatR;

namespace Application.Features.Auth.RefreshToken;

public sealed record RefreshTokenCommand : IRequest<Result<AuthResponseDto>>
{
    public string AccessToken { get; init; } = string.Empty;
}