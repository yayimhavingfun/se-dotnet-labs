using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Services;

public interface ITransactionService
{
    Task<IReadOnlyList<Transaction>> GetHistoryAsync(Guid accountId, CancellationToken ct = default);
}