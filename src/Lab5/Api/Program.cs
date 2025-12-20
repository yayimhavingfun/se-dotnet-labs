using Api.Authentication;
using Core.Application;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructurePostgres();

builder.Services.AddAuthentication("SessionScheme")
    .AddSessionAuthentication();

builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

WebApplication app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        Exception? exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        var response = new
        {
            error = exception?.Message ?? "Unknown error",
            stackTrace = exception?.StackTrace ?? string.Empty,
        };

        await context.Response.WriteAsJsonAsync(response);
    });
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();