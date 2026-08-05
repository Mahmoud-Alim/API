using API.Constants;
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

        var statusCode = result.StatusCode > 0
            ? result.StatusCode
            : StatusCodes.Status400BadRequest;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = GetDefaultTitleForStatusCode(statusCode),
            Detail = result.Error ?? ApiErrors.AnErrorOccurred,
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
        StatusCodes.Status400BadRequest => ProblemDetailsTitles.BadRequest,
        StatusCodes.Status401Unauthorized => ProblemDetailsTitles.Unauthorized,
        StatusCodes.Status403Forbidden => ProblemDetailsTitles.Forbidden,
        StatusCodes.Status404NotFound => ProblemDetailsTitles.NotFound,
        StatusCodes.Status409Conflict => ProblemDetailsTitles.Conflict,
        StatusCodes.Status422UnprocessableEntity => ProblemDetailsTitles.UnprocessableEntity,
        StatusCodes.Status500InternalServerError => ProblemDetailsTitles.InternalServerError,
        _ => ProblemDetailsTitles.Error
    };
}
