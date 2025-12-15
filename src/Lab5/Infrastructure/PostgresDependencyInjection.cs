using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Infrastructure.Postgres;

namespace Itmo.ObjectOrientedProgramming.Lab5.Infrastructure;

public static class PostgresDependencyInjection
{
    public static IServiceCollection AddInfrastructurePostgres(this IServiceCollection services)
    {
        services.AddScoped<IAccountRepository, PostgresAccountRepository>();
        services.AddScoped<ISessionRepository, PostgresSessionRepository>();
        services.AddScoped<ITransactionRepository, PostgresTransactionRepository>();

        return services;
    }
}