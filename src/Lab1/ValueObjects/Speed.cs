namespace Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

public sealed class Speed
{
    public double MetersPerSecond { get; }

    public Speed(double metersPerSecond)
    {
        if (double.IsNaN(metersPerSecond) || double.IsInfinity(metersPerSecond))
            throw new ArgumentException("Speed must be a finite number", nameof(metersPerSecond));

        MetersPerSecond = metersPerSecond;
    }

    public static Speed Zero => new Speed(0);
}
