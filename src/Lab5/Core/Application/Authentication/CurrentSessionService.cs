using Core.Application.Abstractions.Authentication;
using Core.Domain.Entities;

namespace Core.Application.Authentication;

public class CurrentSessionService : ICurrentSessionService
{
    private static readonly AsyncLocal<AtmSession?> Current = new();

    public AtmSession? CurrentSession => Current.Value;

    public void SetCurrentSession(AtmSession atmSession)
    {
        ArgumentNullException.ThrowIfNull(atmSession);
        Current.Value = atmSession;
    }

    public void ClearCurrentSession()
    {
        Current.Value = null;
    }
}