using Core.Application.Abstractions.Authentication;
using Core.Application.Abstractions.Repositories;
using Core.Application.Abstractions.Services;
using Core.Domain.Entities;
using Core.Domain.Results;
using Core.Domain.ValueObjects;
using Microsoft.Extensions.Configuration;

namespace Core.Application.Services;

public class SessionService : ISessionService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly ICurrentSessionService _currentSessionService;
    private readonly IHashingService _hashingService;
    private readonly string _systemPasswordHash;

    public SessionService(
        IAccountRepository accountRepository,
        ISessionRepository sessionRepository,
        ICurrentSessionService currentSessionService,
        IHashingService hashingService,
        IConfiguration configuration)
    {
        _accountRepository = accountRepository;
        _sessionRepository = sessionRepository;
        _currentSessionService = currentSessionService;
        _hashingService = hashingService;

        string? plainPassword = configuration["SystemPassword"];
        if (string.IsNullOrEmpty(plainPassword))
            throw new InvalidOperationException("SystemPassword not configured");

        _systemPasswordHash = hashingService.Hash(plainPassword);
    }

    public async Task<SessionResult> LoginUserAsync(
        AccountNumber number,
        string plainPin,
        CancellationToken ct)
    {
        Account? account = await _accountRepository.FindByNumberAsync(number, ct);

        if (account is null || !_hashingService.Verify(plainPin, account.PinHash))
            return new SessionResult.Failure("INVALID_PIN", "invalid pin");

        var session = AtmSession.CreateForUser(account);

        await _sessionRepository.AddAsync(session, ct);
        _currentSessionService.SetCurrentSession(session);

        return new SessionResult.Success(session);
    }

    public async Task<SessionResult> LoginAdminAsync(string systemPassword, CancellationToken ct)
    {
        if (!_hashingService.Verify(systemPassword, _systemPasswordHash))
            return new SessionResult.Failure("INVALID_PASSWORD", "invalid password");

        var session = AtmSession.CreateForAdmin();
        await _sessionRepository.AddAsync(session, ct);
        _currentSessionService.SetCurrentSession(session);

        return new SessionResult.Success(session);
    }

    public async Task LogoutAsync(Guid sessionId, CancellationToken ct)
    {
        if (_currentSessionService.CurrentSession?.SessionId == sessionId)
            _currentSessionService.ClearCurrentSession();

        await _sessionRepository.DeleteAsync(sessionId, ct);
    }
}