using Application.Common.Models;
using Application.Common.Models.Auth;
using MediatR;

namespace Application.Features.Auth.Register;

public sealed record RegisterCommand : IRequest<Result<AuthResponseDto>>
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}