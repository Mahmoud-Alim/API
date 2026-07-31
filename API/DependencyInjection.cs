using System.Security.Claims;
using System.Text.Json;
using System.Threading.RateLimiting;
using API.Constants;
using API.Middleware;
using API.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace API;

public static class DependencyInjection
{
    /// <summary>Name of the CORS policy registered by <see cref="AddCorsServices"/>.</summary>
    public const string CorsPolicyName = "CorsPolicy";

    private const string UserIdClaimType = ClaimTypes.NameIdentifier;

    private const string AuthenticatedPartitionPrefix = "user:";

    private const string AnonymousPartitionPrefix = "ip:";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Registers all presentation-layer services: MVC, OpenAPI, HttpContextAccessor,
    /// Forwarded Headers, CORS and the Token Bucket Rate Limiter.
    /// </summary>
    public static IServiceCollection AddPresentationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddHttpContextAccessor();

        services.AddForwardedHeadersServices();
        services.AddCorsServices(configuration);
        services.AddRateLimiterServices();

        return services;
    }

    /// <summary>
    /// Registers the presentation-layer middleware pipeline: Forwarded Headers,
    /// exception handling, Serilog request logging, routing, CORS, authentication,
    /// rate limiting and authorization.
    /// </summary>
    public static IApplicationBuilder UsePresentationServices(this IApplicationBuilder app)
    {
        app.UseForwardedHeaders();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseSerilogRequestLogging();

        app.UseRouting();

        app.UseCors(CorsPolicyName);

        app.UseAuthentication();

        app.UseRateLimiter();

        app.UseAuthorization();

        return app;
    }

    /// <summary>
    /// Maps presentation endpoints (controllers and development-only OpenAPI).
    /// </summary>
    public static WebApplication MapPresentationEndpoints(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.MapControllers();

        return app;
    }

    private static IServiceCollection AddForwardedHeadersServices(this IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

            options.KnownIPNetworks.Clear();
            options.KnownProxies.Clear();

            options.ForwardLimit = null;

            options.RequireHeaderSymmetry = false;
        });

        return services;
    }

    /// <summary>
    /// Registers the "CorsPolicy" CORS policy. Allowed origins are read from the
    /// "Cors" section of appsettings.json. All methods and headers are allowed.
    /// Credentials are enabled only when configured AND no wildcard origin is used.
    /// </summary>
    private static IServiceCollection AddCorsServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var corsSettings = configuration
            .GetSection(CorsSettings.SectionName)
            .Get<CorsSettings>() ?? new CorsSettings();

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, policy =>
            {
                if (corsSettings.AllowedOrigins is { Length: > 0 })
                {
                    policy.WithOrigins(corsSettings.AllowedOrigins);
                }
                else
                {
                    policy.AllowAnyOrigin();
                }

                policy.AllowAnyMethod();
                policy.AllowAnyHeader();

                if (corsSettings.AllowCredentials &&
                    corsSettings.AllowedOrigins is { Length: > 0 } &&
                    !corsSettings.AllowedOrigins.Contains("*", StringComparer.Ordinal))
                {
                    policy.AllowCredentials();
                }
            });
        });

        return services;
    }

    private static IServiceCollection AddRateLimiterServices(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                RateLimitPartition.GetSlidingWindowLimiter(
                    partitionKey: GetPartitionKey(context),
                    factory: _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = RateLimiterConstants.GlobalPermitLimit,
                        Window = RateLimiterConstants.GlobalWindow,
                        SegmentsPerWindow = RateLimiterConstants.GlobalSegmentsPerWindow,
                        AutoReplenishment = true
                    }));

            options.AddPolicy(RateLimiterConstants.PolicyName, context =>
            {
                return RateLimitPartition.GetTokenBucketLimiter(
                    partitionKey: GetPartitionKey(context),
                    factory: _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = RateLimiterConstants.TokenLimit,
                        TokensPerPeriod = RateLimiterConstants.TokensPerPeriod,
                        ReplenishmentPeriod = RateLimiterConstants.ReplenishmentPeriod,
                        AutoReplenishment = true,
                        QueueLimit = RateLimiterConstants.QueueLimit,
                        QueueProcessingOrder = RateLimiterConstants.QueueOrder
                    });
            });

            options.OnRejected = async (context, cancellationToken) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.ContentType = "application/json";

                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.HttpContext.Response.Headers.RetryAfter =
                        ((int)retryAfter.TotalSeconds).ToString();
                }

                var payload = new RateLimitExceededResponse
                {
                    Success = false,
                    Message = "Rate limit exceeded. Please try again later."
                };

                await context.HttpContext.Response.WriteAsJsonAsync(
                    payload,
                    JsonOptions,
                    cancellationToken);
            };
        });

        return services;
    }

    private static string GetPartitionKey(HttpContext context)
    {
        var userId = context.User.FindFirstValue(UserIdClaimType);
        if (!string.IsNullOrWhiteSpace(userId))
        {
            return $"{AuthenticatedPartitionPrefix}{userId}";
        }

        var sub = context.User.FindFirstValue("sub");
        if (!string.IsNullOrWhiteSpace(sub))
        {
            return $"{AuthenticatedPartitionPrefix}{sub}";
        }

        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        if (string.IsNullOrWhiteSpace(ipAddress))
        {
            ipAddress = "unknown";
        }

        return $"{AnonymousPartitionPrefix}{ipAddress}";
    }
}

