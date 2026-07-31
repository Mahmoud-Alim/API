using DotNetEnv;
using API;
using Application;
using Infrastructure;
using Serilog;

Env.Load();

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

