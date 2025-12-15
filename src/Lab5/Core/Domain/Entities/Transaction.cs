using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities.Types;
using Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.Entities;

public class Transaction
{
    public Guid Id { get; }

    public Guid AccountId { get; }

    public TransactionType Type { get; }

    public Money Amount { get; }

    public Money NewBalance { get; }

    public DateTime CreatedAt { get; }

    public Transaction(Guid id, Guid accountId, TransactionType type, Money amount, Money newBalance, DateTime createdAt)
    {
        Id = id;
        AccountId = accountId;
        Type = type;
        Amount = amount ?? throw new ArgumentNullException(nameof(amount));
        NewBalance = newBalance ?? throw new ArgumentNullException(nameof(newBalance));
        CreatedAt = createdAt;
    }
}