using Core.Domain.Entities;

namespace Core.Application.Abstractions.Authentication;

public interface ICurrentSessionService
{
    AtmSession? CurrentSession { get; }

    void SetCurrentSession(AtmSession atmSession);

    void ClearCurrentSession();
}