namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Domain.ValueObjects;

public sealed class Money : IEquatable<Money>
{
    public decimal Amount { get; }

    public Money(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive");
        }

        Amount = amount;
    }

    public static Money Zero => new Money(0);

    public static Money operator +(Money a, Money b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);
        return new Money(a.Amount + b.Amount);
    }

    public static Money operator -(Money a, Money b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);
        return new Money(a.Amount - b.Amount);
    }

    public bool IsGreaterThan(Money? other)
    {
        if (other is null) return false;
        return Amount > other.Amount;
    }

    public bool IsLessThan(Money? other)
    {
        if (other is null) return false;
        return Amount < other.Amount;
    }

    public bool IsGreaterThanOrEqual(Money? other)
    {
        if (other is null) return false;
        return Amount >= other.Amount;
    }

    public bool IsLessThanOrEqual(Money? other)
    {
        if (other is null) return false;
        return Amount <= other.Amount;
    }

    public bool Equals(Money? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Amount == other.Amount;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Money);
    }

    public override int GetHashCode()
    {
        return Amount.GetHashCode();
    }

    public static bool operator ==(Money? left, Money? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(Money? left, Money? right) => !(left == right);
}