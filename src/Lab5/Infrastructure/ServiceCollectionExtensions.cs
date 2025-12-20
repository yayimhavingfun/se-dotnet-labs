using Core.Application.Abstractions.Repositories;
using Infrastructure.Postgres;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructurePostgres(this IServiceCollection services)
    {
        services.AddScoped<IAccountRepository, PostgresAccountRepository>();
        services.AddScoped<ISessionRepository, PostgresSessionRepository>();
        services.AddScoped<ITransactionRepository, PostgresTransactionRepository>();

        return services;
    }
}