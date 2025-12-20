using Core.Domain.ValueObjects;

namespace Core.Domain.Entities;

public class AtmSession
{
    public Guid SessionId { get; }

    public DateTime CreatedAt { get; }

    public Account? Account { get; private init; }

    public bool IsAdmin { get; private set; }

    public AtmSession(Guid sessionId, bool isAdmin, Account? account, DateTime createdAt)
    {
        SessionId = sessionId;
        IsAdmin = isAdmin;
        Account = account;
        CreatedAt = createdAt;
    }

    public static AtmSession CreateForAdmin()
    {
        return new AtmSession(Guid.NewGuid(), isAdmin: true, account: null, DateTime.UtcNow);
    }

    public static AtmSession CreateForUser(Account account)
    {
        ArgumentNullException.ThrowIfNull(account);
        return new AtmSession(Guid.NewGuid(), isAdmin: false, account: account, DateTime.UtcNow);
    }

    public Account RequireAccount()
    {
        return Account ?? throw new InvalidOperationException("This session does not belong to a user");
    }

    public Guid AccountId => RequireAccount().Id;

    public AccountNumber AccountNumber => RequireAccount().AccountNumber;
}