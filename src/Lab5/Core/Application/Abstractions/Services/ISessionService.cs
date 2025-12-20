using Core.Domain.Results;
using Core.Domain.ValueObjects;

namespace Core.Application.Abstractions.Services;

public interface ISessionService
{
    Task<SessionResult> LoginUserAsync(AccountNumber number, string plainPin, CancellationToken ct);

    Task<SessionResult> LoginAdminAsync(string systemPassword, CancellationToken ct);

    Task LogoutAsync(Guid sessionId, CancellationToken ct);
}