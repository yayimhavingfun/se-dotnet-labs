using Core.Domain.Results;
using Core.Domain.ValueObjects;

namespace Core.Application.Abstractions.Services;

public interface IAccountService
{
    Task<TransactionResult> DepositAsync(AccountNumber number, Money amount, CancellationToken ct);

    Task<TransactionResult> WithdrawAsync(AccountNumber number, Money amount, CancellationToken ct);

    Task<BalanceResult> GetBalanceAsync(AccountNumber number, CancellationToken ct);
}