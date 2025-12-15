using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Results;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Services;

public interface ISessionService
{
    Task<SessionResult> LoginUserAsync(AccountNumber number, string plainPin, CancellationToken ct = default);

    Task<SessionResult> LoginAdminAsync(string systemPassword, CancellationToken ct = default);

    Task LogoutAsync(Guid sessionId, CancellationToken ct = default);
}