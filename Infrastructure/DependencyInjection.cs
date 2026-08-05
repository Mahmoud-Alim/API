using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Settings;
using Infrastructure.Configuration;
using Infrastructure.Constants;
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
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(JwtSettings.SectionName))
            .Validate(settings =>
            {
                ValidateJwtSettings(settings);
                return true;
            })
            .ValidateOnStart();

        services.AddOptions<IdentitySettings>()
            .Bind(configuration.GetSection(IdentitySettings.SectionName))
            .ValidateOnStart();

services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString(ConnectionStrings.DefaultConnection)));

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
        {
            var identitySettings = configuration
                .GetSection(IdentitySettings.SectionName)
                .Get<IdentitySettings>() ?? new IdentitySettings();

            options.Password.RequireDigit = identitySettings.PasswordRequireDigit;
            options.Password.RequireLowercase = identitySettings.PasswordRequireLowercase;
            options.Password.RequireUppercase = identitySettings.PasswordRequireUppercase;
            options.Password.RequireNonAlphanumeric = identitySettings.PasswordRequireNonAlphanumeric;
            options.Password.RequiredLength = identitySettings.PasswordRequiredLength;
            options.User.RequireUniqueEmail = identitySettings.UserRequireUniqueEmail;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(
                identitySettings.LockoutDefaultLockoutTimeSpanMinutes);
            options.Lockout.MaxFailedAccessAttempts = identitySettings.LockoutMaxFailedAccessAttempts;
            options.Lockout.AllowedForNewUsers = identitySettings.LockoutAllowedForNewUsers;
            options.User.AllowedUserNameCharacters = identitySettings.AllowedUserNameCharacters;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders()
        .AddErrorDescriber<CustomIdentityErrorDescriber>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer();

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .PostConfigure<IOptions<JwtSettings>>(
                (options, jwtSettingsOptions) =>
                {
                    var jwtSettings = jwtSettingsOptions.Value;

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
            options.AddPolicy(AuthPolicies.RequireBossRole, policy =>
                policy.RequireRole(Roles.Boss));

            options.AddPolicy(AuthPolicies.RequireAdminRole, policy =>
                policy.RequireRole(Roles.Admin));

            options.AddPolicy(AuthPolicies.RequireUserRole, policy =>
                policy.RequireRole(Roles.User));
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IAuditService, AuditService>();

        return services;
    }

private static ILogger GetJwtLogger(IServiceProvider services)
    {
        var factory = services.GetRequiredService<ILoggerFactory>();
        return factory.CreateLogger(LoggerCategories.JwtBearer);
    }

private static void ValidateJwtSettings(JwtSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.SecretKey))
        {
            throw new InvalidOperationException(
                JwtValidationMessages.SecretKeyNotConfigured);
        }

        if (settings.SecretKey.Length < TokenConstants.JwtSecretKeyMinLength)
        {
            throw new InvalidOperationException(
                $"{JwtValidationMessages.SecretKeyTooShort} " +
                $"Current length: {settings.SecretKey.Length} characters.");
        }

        if (string.IsNullOrWhiteSpace(settings.Issuer))
        {
            throw new InvalidOperationException(
                JwtValidationMessages.IssuerNotConfigured);
        }

        if (string.IsNullOrWhiteSpace(settings.Audience))
        {
            throw new InvalidOperationException(
                JwtValidationMessages.AudienceNotConfigured);
        }

        if (settings.AccessTokenExpirationMinutes <= 0)
        {
            throw new InvalidOperationException(
                JwtValidationMessages.AccessTokenExpirationInvalid);
        }

        if (settings.RefreshTokenExpirationDays <= 0)
        {
            throw new InvalidOperationException(
                JwtValidationMessages.RefreshTokenExpirationInvalid);
        }
    }
}
