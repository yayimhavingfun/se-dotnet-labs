using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Repositories;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken ct = default);

    Task<IReadOnlyList<Transaction>> GetByAccountIdAsync(Guid accountId, CancellationToken ct = default);
}