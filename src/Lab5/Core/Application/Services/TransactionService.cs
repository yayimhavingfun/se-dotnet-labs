using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Repositories;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Services;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<IReadOnlyList<Transaction>> GetHistoryAsync(
        Guid accountId,
        CancellationToken ct = default)
    {
        return await _transactionRepository.GetByAccountIdAsync(accountId, ct);
    }
}