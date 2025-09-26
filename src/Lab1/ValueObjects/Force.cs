namespace Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

public sealed class Force
{
    public double Newtons { get; }

    public Force(double newtons)
    {
        Newtons = newtons;
    }
}