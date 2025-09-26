namespace Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

public sealed class Distance
{
    public double Meters { get; }

    public Distance(double meters)
    {
        if (meters < 0) throw new ArgumentException("Distance cannot be negative");
        Meters = meters;
    }

    public static Distance Zero => new Distance(0);
}
