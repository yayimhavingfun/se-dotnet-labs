using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Authentication;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Services;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Results;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Services;

public class SessionService : ISessionService
{
    private readonly IAccountRepository _accountRepo;
    private readonly ISessionRepository _sessionRepo;
    private readonly ICurrentSessionService _currentSession;
    private readonly IHashingService _hashing;
    private readonly string _systemPasswordHash;

    public SessionService(
        IAccountRepository accountRepo,
        ISessionRepository sessionRepo,
        ICurrentSessionService currentSession,
        IHashingService hashing,
        IConfiguration configuration)
    {
        _accountRepo = accountRepo;
        _sessionRepo = sessionRepo;
        _currentSession = currentSession;
        _hashing = hashing;

        string? plainPassword = configuration["SystemPassword"];
        if (string.IsNullOrEmpty(plainPassword))
            throw new InvalidOperationException("SystemPassword not configured");

        _systemPasswordHash = hashing.Hash(plainPassword);
    }

    public async Task<SessionResult> LoginUserAsync(
        AccountNumber number,
        string plainPin,
        CancellationToken ct = default)
    {
        Account? account = await _accountRepo.FindByNumberAsync(number, ct);
        if (account is null || !_hashing.Verify(plainPin, account.PinHash))
            return new SessionResult.Failure("INVALID_PIN", "invalid pin");

        var session = AtmSession.CreateForUser(account);
        await _sessionRepo.AddAsync(session, ct);
        _currentSession.SetCurrentSession(session);

        return new SessionResult.Success(session);
    }

    public async Task<SessionResult> LoginAdminAsync(string systemPassword, CancellationToken ct = default)
    {
        if (!_hashing.Verify(systemPassword, _systemPasswordHash))
            return new SessionResult.Failure("INVALID_PASSWORD", "invalid password");

        var session = AtmSession.CreateForAdmin();
        await _sessionRepo.AddAsync(session, ct);
        _currentSession.SetCurrentSession(session);

        return new SessionResult.Success(session);
    }

    public async Task LogoutAsync(Guid sessionId, CancellationToken ct = default)
    {
        if (_currentSession.CurrentSession?.SessionId == sessionId)
            _currentSession.ClearCurrentSession();

        await _sessionRepo.DeleteAsync(sessionId, ct);
    }
}