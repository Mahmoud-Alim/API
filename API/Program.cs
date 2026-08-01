using DotNetEnv;
using API;
using Application;
using Infrastructure;
using Serilog;

var envFilePath = FindEnvFile();
if (envFilePath is not null)
{
    Env.Load(envFilePath);
}
else
{
    Env.Load();
}

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPresentationServices(builder.Configuration);

var app = builder.Build();

app.UsePresentationServices();

app.MapPresentationEndpoints();

app.Run();

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

