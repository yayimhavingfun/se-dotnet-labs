using Core.Domain.Entities;

namespace Core.Application.Abstractions.Repositories;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken ct);

    Task<IReadOnlyList<Transaction>> GetByAccountIdAsync(Guid accountId, CancellationToken ct);

    Task DeleteByAccountIdAsync(Guid accountId, CancellationToken ct);
}