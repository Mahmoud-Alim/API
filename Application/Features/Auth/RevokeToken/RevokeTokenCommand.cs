using Application.Common.Models;
using MediatR;

namespace Application.Features.Auth.RevokeToken;

public sealed record RevokeTokenCommand : IRequest<Result<bool>>
{
    public string Token { get; init; } = string.Empty;
}