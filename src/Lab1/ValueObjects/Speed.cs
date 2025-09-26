namespace Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

public sealed class Speed
{
    public double MetersPerSecond { get; }

    public Speed(double metersPerSecond)
    {
        MetersPerSecond = metersPerSecond;
    }

    public static Speed Zero => new Speed(0);
}
