using DotNetEnv;
using API;
using Application;
using Infrastructure;
using Serilog;

// Load the .env file from the repository root even when the app is launched
// from a different working directory (e.g. Visual Studio, dotnet run from the
// API folder, or from a build output folder).
var envFilePath = FindEnvFile();
if (envFilePath is not null)
{
    Env.Load(envFilePath);
}
else
{
    // Fall back to DotNetEnv's default behaviour (searches from CWD upward).
    Env.Load();
}

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Registers Application, Infrastructure and Presentation (MVC, OpenAPI,
// Forwarded Headers, CORS and Rate Limiting) services via extension methods.
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPresentationServices(builder.Configuration);

var app = builder.Build();

// Registers the presentation middleware pipeline (Forwarded Headers, Exception
// handling, Serilog logging, Routing, CORS, Authentication, Rate Limiting and
// Authorization) via extension methods.
app.UsePresentationServices();

// Maps controllers and development-only OpenAPI via extension methods.
app.MapPresentationEndpoints();

app.Run();

/// <summary>
/// Searches the current working directory, the application base directory and
/// every parent directory for a .env file, returning the first match.
/// </summary>
static string? FindEnvFile()
{
    var startingPoints = new[]
    {
        Directory.GetCurrentDirectory(),
        AppContext.BaseDirectory
    };

    foreach (var start in startingPoints.Distinct(StringComparer.Ordinal))
    {
        var directory = new DirectoryInfo(start);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, ".env");
            if (File.Exists(candidate))
            {
                return candidate;
            }

            directory = directory.Parent;
        }
    }

    return null;
}

