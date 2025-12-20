using Core.Domain.Entities;
using Core.Domain.ValueObjects;

namespace Core.Application.Abstractions.Repositories;

public interface IAccountRepository
{
    Task<Account?> FindByNumberAsync(AccountNumber number, CancellationToken ct);

    Task AddAsync(Account account, CancellationToken ct);

    Task UpdateAsync(Account account, CancellationToken ct);

    Task DeleteAsync(Guid accountId, CancellationToken ct);
}