namespace Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

public sealed class Force
{
    public double Newtons { get; }

    public Force(double newtons)
    {
        Newtons = newtons;

        if (double.IsNaN(newtons) || double.IsInfinity(newtons))
            throw new ArgumentException("Force value must be a finite number", nameof(newtons));
    }
}