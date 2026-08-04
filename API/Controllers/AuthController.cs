using API.Constants;
using API.Middleware;
using Domain.Settings;
using Application.Common.Models.Auth;
using Application.Features.Auth.Register;
using Application.Features.Auth.Login;
using Application.Features.Auth.RefreshTokenFeature;
using Application.Features.Auth.RevokeToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers;

[ApiController]
[Route(RouteConstants.Auth.Base)]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly JwtSettings _jwtSettings;

    public AuthController(IMediator mediator, IOptions<JwtSettings> jwtSettings)
    {
        _mediator = mediator;
        _jwtSettings = jwtSettings.Value;
    }

    [HttpPost(RouteConstants.Auth.Register)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponseDto>> Register(
        [FromBody] RegisterRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCommand
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = request.Password
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.ToActionResult(this);
        }

        return result.ToCreatedActionResult(this, nameof(Login), new { });
    }

    [HttpPost(RouteConstants.Auth.Login)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> Login(
        [FromBody] LoginRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand
        {
            Email = request.Email,
            Password = request.Password
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.ToActionResult(this);
        }

        if (result.Data is not null)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(
                    _jwtSettings.RefreshTokenExpirationDays)
            };

            Response.Cookies.Append(ApiHeaders.RefreshToken, result.Data.RefreshToken, cookieOptions);
        }

        return result.ToActionResult(this);
    }

    [HttpPost(RouteConstants.Auth.RefreshToken)]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken(
        [FromBody] RefreshTokenRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand
        {
            AccessToken = request.AccessToken
        };

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return result.ToActionResult(this);
        }

        if (result.Data is not null)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(
                    _jwtSettings.RefreshTokenExpirationDays)
            };

            Response.Cookies.Append(ApiHeaders.RefreshToken, result.Data.RefreshToken, cookieOptions);
        }

        return result.ToActionResult(this);
    }

    [HttpPost(RouteConstants.Auth.Logout)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<bool>> RevokeToken(
        CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies[ApiHeaders.RefreshToken];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = ApiErrors.NoRefreshToken
            });
        }

        var command = new RevokeTokenCommand { Token = refreshToken };
        var result = await _mediator.Send(command, cancellationToken);

        return result.ToActionResult(this);
    }
}
