using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Application.Abstractions.Authentication;

public interface ICurrentSessionService
{
    AtmSession? CurrentSession { get; }

    void SetCurrentSession(AtmSession atmSession);

    void ClearCurrentSession();
}