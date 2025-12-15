using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Repositories;

public interface ISessionRepository
{
    Task<AtmSession?> FindByIdAsync(Guid sessionId, CancellationToken ct = default);

    Task AddAsync(AtmSession atmSession, CancellationToken ct = default);

    Task DeleteAsync(Guid sessionId, CancellationToken ct = default);
}