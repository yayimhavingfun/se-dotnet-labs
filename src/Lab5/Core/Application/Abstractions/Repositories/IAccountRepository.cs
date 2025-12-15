using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Repositories;

public interface IAccountRepository
{
    Task<Account?> FindByNumberAsync(AccountNumber number, CancellationToken ct = default);

    Task AddAsync(Account account, CancellationToken ct = default);

    Task UpdateAsync(Account account, CancellationToken ct = default);
}