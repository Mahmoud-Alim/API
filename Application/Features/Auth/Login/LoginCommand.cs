using Application.Common.Models;
using Application.Common.Models.Auth;
using MediatR;

namespace Application.Features.Auth.Login;

public sealed record LoginCommand : IRequest<Result<AuthResponseDto>>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}