using Core.Application.Abstractions.Authentication;
using Core.Application.Abstractions.Repositories;
using Core.Application.Abstractions.Services;
using Core.Domain.Entities;
using Core.Domain.Results;
using Core.Domain.ValueObjects;

namespace Core.Application.Services;

public class AdminService : IAdminService
{
    private readonly IAccountRepository _accountRepository;

    public AdminService(IHashingService hashingService, IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountResult> CreateAccountAsync(
        AccountNumber number,
        string plainPin,
        Money initialDeposit,
        CancellationToken ct = default)
    {
        Account? existing = await _accountRepository.FindByNumberAsync(number, ct);
        if (existing is not null)
            return new AccountResult.Failure("ACCOUNT_EXISTS", "Account already exists");

        var account = new Account(number, plainPin, initialDeposit);

        await _accountRepository.AddAsync(account, ct);

        return new AccountResult.Success(number);
    }
}