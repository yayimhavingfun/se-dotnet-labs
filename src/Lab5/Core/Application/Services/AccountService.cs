using Core.Application.Abstractions.Repositories;
using Core.Application.Abstractions.Services;
using Core.Domain.Entities;
using Core.Domain.Results;
using Core.Domain.ValueObjects;

namespace Core.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    public AccountService(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<TransactionResult> DepositAsync(
        AccountNumber number,
        Money amount,
        CancellationToken ct)
    {
        Account account = await _accountRepository.FindByNumberAsync(number, ct)
                          ?? throw new KeyNotFoundException($"Account with number {number} not found");

        TransactionResult result = account.Deposit(amount);

        if (result is TransactionResult.Success success)
        {
            await _transactionRepository.AddAsync(success.Transaction, ct);
            await _accountRepository.UpdateAsync(account, ct);
        }

        return result;
    }

    public async Task<TransactionResult> WithdrawAsync(
        AccountNumber number,
        Money amount,
        CancellationToken ct = default)
    {
        Account account = await _accountRepository.FindByNumberAsync(number, ct)
                          ?? throw new KeyNotFoundException($"Account with number {number} not found");

        TransactionResult result = account.Withdraw(amount);

        if (result is TransactionResult.Success { Transaction: var transaction })
        {
            await _transactionRepository.AddAsync(transaction, ct);
            await _accountRepository.UpdateAsync(account, ct);
        }

        return result;
    }

    public async Task<BalanceResult> GetBalanceAsync(AccountNumber number, CancellationToken ct)
    {
        Account? account = await _accountRepository.FindByNumberAsync(number, ct);

        return account is not null
            ? account.ViewBalance()
            : new BalanceResult.Failure("NOT_FOUND", $"Account with number {number} not found");
    }
}