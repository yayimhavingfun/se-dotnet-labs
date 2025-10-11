namespace Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

public sealed class Time
{
    public double Seconds { get; }

    public Time(double seconds)
    {
        if (double.IsNaN(seconds) || double.IsInfinity(seconds))
            throw new ArgumentException("Time must be a finite number", nameof(seconds));

        if (seconds < 0) throw new ArgumentException("Time cannot be negative");
        Seconds = seconds;
    }

    public static Time Zero => new Time(0);

    public Time Add(Time other) => new Time(Seconds + other.Seconds);
}
