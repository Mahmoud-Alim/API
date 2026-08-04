using API;
using Application;
using Infrastructure;
using Serilog;

EnvFileLoader.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddPresentationServices(builder.Configuration);

var app = builder.Build();

app.UsePresentationServices();

app.MapPresentationEndpoints();

app.Run();

