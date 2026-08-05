using System.Diagnostics;
using Application.Common.Constants;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;

public sealed class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        var stopwatch = Stopwatch.StartNew();

        var response = await next(cancellationToken);

        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > PerformanceConstants.ThresholdMilliseconds)
        {
            _logger.LogWarning(
                "Long running request {RequestName} took {ElapsedMilliseconds}ms (threshold: {Threshold}ms)",
                requestName,
                stopwatch.ElapsedMilliseconds,
                PerformanceConstants.ThresholdMilliseconds);
        }

        return response;
    }
}

