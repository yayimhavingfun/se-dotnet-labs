using Core.Application.Abstractions.Repositories;
using Core.Application.Abstractions.Services;
using Core.Domain.Entities;

namespace Core.Application.Services;

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