using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Authentication;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Services;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Results;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Services;

public class AdminService : IAdminService
{
    private readonly IHashingService _hashingService;
    private readonly IAccountRepository _accountRepository;

    public AdminService(IHashingService hashingService, IAccountRepository accountRepository)
    {
        _hashingService = hashingService;
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

        string pin = _hashingService.Hash(plainPin);

        var account = new Account(number, pin, initialDeposit);

        await _accountRepository.AddAsync(account, ct);

        return new AccountResult.Success(number);
    }
}