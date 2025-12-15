using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Services;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Results;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepo;
    private readonly ITransactionRepository _transactionRepo;

    public AccountService(IAccountRepository accountRepo, ITransactionRepository transactionRepo)
    {
        _accountRepo = accountRepo;
        _transactionRepo = transactionRepo;
    }

    public async Task<TransactionResult> DepositAsync(
        AccountNumber number,
        Money amount,
        CancellationToken ct = default)
    {
        Account account = await _accountRepo.FindByNumberAsync(number, ct)
                          ?? throw new KeyNotFoundException($"Account with number {number} not found");

        TransactionResult result = account.Deposit(amount);

        if (result is TransactionResult.Success success)
        {
            await _transactionRepo.AddAsync(success.Transaction, ct);
            await _accountRepo.UpdateAsync(account, ct);
        }

        return result;
    }

    public async Task<TransactionResult> WithdrawAsync(
        AccountNumber number,
        Money amount,
        CancellationToken ct = default)
    {
        Account account = await _accountRepo.FindByNumberAsync(number, ct)
                          ?? throw new KeyNotFoundException($"Account with number {number} not found");

        TransactionResult result = account.Withdraw(amount);

        if (result is TransactionResult.Success { Transaction: var transaction })
        {
            await _transactionRepo.AddAsync(transaction, ct);
            await _accountRepo.UpdateAsync(account, ct);
        }

        return result;
    }

    public async Task<BalanceResult> GetBalanceAsync(AccountNumber number, CancellationToken ct = default)
    {
        Account? account = await _accountRepo.FindByNumberAsync(number, ct);

        return account is not null
            ? account.ViewBalance()
            : new BalanceResult.Failure("NOT_FOUND", $"Account with number {number} not found");
    }
}