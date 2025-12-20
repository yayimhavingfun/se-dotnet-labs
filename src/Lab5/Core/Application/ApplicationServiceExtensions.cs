using Core.Application.Abstractions.Authentication;
using Core.Application.Abstractions.Services;
using Core.Application.Authentication;
using Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<ITransactionService, TransactionService>();

        services.AddScoped<ICurrentSessionService, CurrentSessionService>();
        services.AddSingleton<IHashingService, HashingService>();
        return services;
    }
}