using Itmo.ObjectOrientedProgramming.Lab5.Api.Middleware;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Application;
using Itmo.ObjectOrientedProgramming.Lab5.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructurePostgres();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSessionAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();