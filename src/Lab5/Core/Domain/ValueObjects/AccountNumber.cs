namespace Core.Domain.ValueObjects;

public sealed class AccountNumber : IEquatable<AccountNumber>
{
    public string Value { get; }

    public AccountNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Account value is required", nameof(value));

        if (value.Length != 10)
            throw new ArgumentException("Account value must be exactly 10 digits", nameof(value));

        if (!value.All(char.IsDigit))
            throw new ArgumentException("Account value must contain only digits", nameof(value));

        Value = value;
    }

    public bool Equals(AccountNumber? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as AccountNumber);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode(StringComparison.Ordinal);
    }

    public static bool operator ==(AccountNumber? left, AccountNumber? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(AccountNumber? left, AccountNumber? right) => !(left == right);

    public bool Equals(string accountNumber)
    {
        return Value == accountNumber;
    }
}