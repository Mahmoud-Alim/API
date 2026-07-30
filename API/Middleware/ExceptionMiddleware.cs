using System.Diagnostics;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace API.Middleware;

public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Prevent response tampering if headers have already been sent.
            if (context.Response.HasStarted)
            {
                _logger.LogWarning(
                    ex,
                    "An exception occurred after the response had already started; " +
                    "the partial response has been sent as-is.");
                throw;
            }

            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var problemDetails = new ProblemDetails
        {
            Instance = context.Request.Path,
            Extensions = { ["traceId"] = Activity.Current?.Id ?? context.TraceIdentifier }
        };

        switch (exception)
        {
            case ValidationException validationEx:
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Validation Failed";
                problemDetails.Detail = "One or more validation errors occurred.";
                problemDetails.Extensions["errors"] = validationEx.Errors;

                _logger.LogWarning(
                    validationEx,
                    "Request validation failed for {Path}",
                    context.Request.Path);
                break;

            case NotFoundException:
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Title = "Resource Not Found";
                problemDetails.Detail = exception.Message;

                _logger.LogWarning(
                    exception,
                    "Resource not found at {Path}",
                    context.Request.Path);
                break;

            case UnauthorizedException:
            case UnauthorizedAccessException:
                problemDetails.Status = StatusCodes.Status401Unauthorized;
                problemDetails.Title = "Unauthorized";
                problemDetails.Detail = "You are not authorized to access this resource.";

                _logger.LogWarning(
                    exception,
                    "Unauthorized access attempt to {Path}",
                    context.Request.Path);
                break;

            case ForbiddenException:
                problemDetails.Status = StatusCodes.Status403Forbidden;
                problemDetails.Title = "Forbidden";
                problemDetails.Detail = "You do not have permission to access this resource.";

                _logger.LogWarning(
                    exception,
                    "Forbidden access attempt to {Path}",
                    context.Request.Path);
                break;

            case ConflictException:
                problemDetails.Status = StatusCodes.Status409Conflict;
                problemDetails.Title = "Conflict";
                problemDetails.Detail = exception.Message;

                _logger.LogWarning(
                    exception,
                    "Resource conflict at {Path}",
                    context.Request.Path);
                break;

            default:
                // Unexpected / infrastructure error — keep detail opaque to the client.
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Internal Server Error";
                problemDetails.Detail = "An unexpected error occurred. Please try again later.";

                _logger.LogError(
                    exception,
                    "Unhandled exception processing {Path}: {ExceptionType}",
                    context.Request.Path,
                    exception.GetType().Name);
                break;
        }

        context.Response.StatusCode = problemDetails.Status!.Value;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(
            problemDetails,
            JsonSerializerOptionsProvider.Default,
            context.RequestAborted);
    }
}

internal static class JsonSerializerOptionsProvider
{
    public static readonly System.Text.Json.JsonSerializerOptions Default = new()
    {
        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };
}

