using API.Middleware;
using Application;
using Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.
    AddApplication()
   .AddInfrastructure(builder.Configuration);
   
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseSerilogRequestLogging();

// app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
