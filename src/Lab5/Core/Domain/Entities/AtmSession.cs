using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;

public sealed class AtmSession
{
    public Guid SessionId { get; }

    public DateTime CreatedAt { get; }

    public Account? Account { get; private init; }

    public bool IsAdmin { get; private init; }

    private AtmSession()
    {
        SessionId = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public static AtmSession CreateForUser(Account account)
    {
        ArgumentNullException.ThrowIfNull(account, nameof(account));

        return new AtmSession() { Account = account };
    }

    public static AtmSession CreateForAdmin()
    {
        return new AtmSession() { IsAdmin = true };
    }

    public Account RequireAccount()
    {
        return Account ?? throw new InvalidOperationException("This session does not belong to a user");
    }

    public Guid AccountId => RequireAccount().Id;

    public AccountNumber AccountNumber => RequireAccount().AccountNumber;
}