namespace Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

public sealed class Mass
{
    public double Kilograms { get; }

    public Mass(double kilograms)
    {
        if (kilograms <= 0) throw new ArgumentException("Mass must be positive");
        Kilograms = kilograms;
    }
}
