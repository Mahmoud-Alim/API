using API.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace API.Controllers;

[ApiController]
[Route(RouteConstants.RateLimitDemo.Base)]
public sealed class RateLimitDemoController : ControllerBase
{
    [HttpGet]
    [EnableRateLimiting(RateLimiterConstants.PolicyName)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
public ActionResult<string> Get()
    {
        return Ok(ApiErrors.RateLimitEndpointReached);
    }
}

