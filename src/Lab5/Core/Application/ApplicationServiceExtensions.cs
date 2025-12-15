using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Authentication;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Services;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Authentication;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Services;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Application;

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