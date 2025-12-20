using Core.Domain.Entities;

namespace Core.Application.Abstractions.Services;

public interface ITransactionService
{
    Task<IReadOnlyList<Transaction>> GetHistoryAsync(Guid accountId, CancellationToken ct);
}