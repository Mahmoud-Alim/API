using Domain.Entities;
using Domain.Interfaces;
using Domain.Settings;
using Infrastructure.IdentityHelper;
using Infrastructure.implementation;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders()
        .AddErrorDescriber<CustomIdentityErrorDescriber>();
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>()
            ?? throw new InvalidOperationException("JWT settings are not configured.");

        // Fail fast with a clear message instead of the cryptic
        // "IDX10703: key length is zero" thrown by SymmetricSecurityKey.
        ValidateJwtSettings(jwtSettings);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                RequireSignedTokens = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                ClockSkew = TimeSpan.Zero,
                ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 }
            };
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    var logger = GetJwtLogger(context.HttpContext.RequestServices);
                    logger.LogWarning("JWT authentication failed: {Error}", context.Exception.Message);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    var logger = GetJwtLogger(context.HttpContext.RequestServices);
                    logger.LogInformation("JWT token validated for user {User}", context.Principal?.Identity?.Name);
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    var logger = GetJwtLogger(context.HttpContext.RequestServices);
                    logger.LogWarning("JWT challenge triggered for path {Path}", context.HttpContext.Request.Path);
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireBossRole", policy =>
                policy.RequireRole("Boss"));

            options.AddPolicy("RequireAdminRole", policy =>
                policy.RequireRole("Admin"));

            options.AddPolicy("RequireUserRole", policy =>
                policy.RequireRole("User"));
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IAuditService, AuditService>();

        return services;
    }

    /// <summary>
    /// Resolves a named <see cref="ILogger"/> for JwtBearer events.
    /// The non-generic <c>ILogger</c> is not registered by the logging host
    /// (only <c>ILogger<T></c> and <c>ILoggerFactory</c> are), so we
    /// create one from the factory to avoid a runtime
    /// <see cref="InvalidOperationException"/> when a challenge fires.
    /// </summary>
    private static ILogger GetJwtLogger(IServiceProvider services)
    {
        var factory = services.GetRequiredService<ILoggerFactory>();
        return factory.CreateLogger("JwtBearer");
    }

    /// <summary>
    /// Ensures all JWT settings are present and valid before they are used to
    /// build the signing key. Without this, an empty secret produces the
    /// confusing "IDX10703: key length is zero" at the first request.
    /// </summary>
    private static void ValidateJwtSettings(JwtSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.SecretKey))
        {
            throw new InvalidOperationException(
                "JWT SecretKey is not configured. Set 'Jwt__SecretKey' in the .env file " +
                "(or 'Jwt:SecretKey' in appsettings.json) to a value of at least 32 characters (256 bits).");
        }

        if (settings.SecretKey.Length < 32)
        {
            throw new InvalidOperationException(
                $"JWT SecretKey is too short ({settings.SecretKey.Length} characters). " +
                "HS256 requires a key of at least 32 characters (256 bits).");
        }

        if (string.IsNullOrWhiteSpace(settings.Issuer))
        {
            throw new InvalidOperationException(
                "JWT Issuer is not configured. Set 'Jwt__Issuer' in the .env file " +
                "(or 'Jwt:Issuer' in appsettings.json).");
        }

        if (string.IsNullOrWhiteSpace(settings.Audience))
        {
            throw new InvalidOperationException(
                "JWT Audience is not configured. Set 'Jwt__Audience' in the .env file " +
                "(or 'Jwt:Audience' in appsettings.json).");
        }

        if (settings.AccessTokenExpirationMinutes <= 0)
        {
            throw new InvalidOperationException(
                "JWT AccessTokenExpirationMinutes must be greater than zero. " +
                "Set 'Jwt__AccessTokenExpirationMinutes' in the .env file.");
        }

        if (settings.RefreshTokenExpirationDays <= 0)
        {
            throw new InvalidOperationException(
                "JWT RefreshTokenExpirationDays must be greater than zero. " +
                "Set 'Jwt__RefreshTokenExpirationDays' in the .env file.");
        }
    }
}
