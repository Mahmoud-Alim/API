using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Middleware;

public static class ResultExtensions
{
    public static ActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
    {
        if (result.IsSuccess)
        {
            return result.Data is null
                ? controller.NoContent()
                : controller.Ok(result.Data);
        }

        var statusCode = result.StatusCode > 0 ? result.StatusCode : 400;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = GetDefaultTitleForStatusCode(statusCode),
            Detail = result.Error ?? "An error occurred.",
            Instance = controller.Request?.Path
        };

        return controller.StatusCode(statusCode, problemDetails);
    }

    public static ActionResult ToCreatedActionResult<T>(this Result<T> result, ControllerBase controller, string actionName, object routeValues)
    {
        if (result.IsSuccess)
        {
            return controller.CreatedAtAction(actionName, routeValues, result.Data);
        }

        return result.ToActionResult(controller);
    }

    private static string GetDefaultTitleForStatusCode(int statusCode) => statusCode switch
    {
        400 => "Bad Request",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "Not Found",
        409 => "Conflict",
        422 => "Unprocessable Entity",
        500 => "Internal Server Error",
        _ => "Error"
    };
}

