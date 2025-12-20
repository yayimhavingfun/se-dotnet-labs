using Core.Domain.Entities;

namespace Core.Application.Abstractions.Repositories;

public interface ISessionRepository
{
    Task<AtmSession?> FindByIdAsync(Guid sessionId, CancellationToken ct);

    Task AddAsync(AtmSession atmSession, CancellationToken ct);

    Task DeleteAsync(Guid sessionId, CancellationToken ct);
}