namespace Core.Domain.ValueObjects;

public sealed class PinCode : IEquatable<PinCode>
{
    public int Value { get; }

    public PinCode(int value)
    {
        if (value < 0 || value > 9999)
            throw new ArgumentException("PIN must be between 0000 and 9999", nameof(value));

        Value = value;
    }

    public string Formatted => Value.ToString("D4");

    public bool Matches(string input)
    {
        return int.TryParse(input, out int inputPin) && inputPin == Value;
    }

    public bool Matches(int inputPin) => inputPin == Value;

    public bool Equals(PinCode? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as PinCode);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(PinCode? left, PinCode? right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(PinCode? left, PinCode? right)
    {
        return !(left == right);
    }
}