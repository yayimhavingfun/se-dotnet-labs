using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Results;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Services;

public interface IAccountService
{
    Task<TransactionResult> DepositAsync(AccountNumber number, Money amount, CancellationToken ct = default);

    Task<TransactionResult> WithdrawAsync(AccountNumber number, Money amount, CancellationToken ct = default);

    Task<BalanceResult> GetBalanceAsync(AccountNumber number, CancellationToken ct = default);
}