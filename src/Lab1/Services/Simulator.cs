using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Services;

public class Simulator
{
    public double CalculateAcceleration(Force force, Mass mass)
    => force.Newtons / mass.Kilograms;

    public double CalculateBrakingDistance(Speed speed, double acceleration)
    {
        if (speed.MetersPerSecond <= 0 || acceleration >= 0) return 0;
        return Math.Pow(speed.MetersPerSecond, 2) / (2 * Math.Abs(acceleration));
    }

    public Time CalculateTimeToStop(Speed speed, double deceleration)
    {
        if (speed.MetersPerSecond <= 0 || deceleration >= 0) return Time.Zero;
        return new Time(speed.MetersPerSecond / Math.Abs(deceleration));
    }

    public bool CanApplyForce(double force, double maxForce)
        => Math.Abs(force) <= maxForce;

    public Time CalculateTimeToReachSpeed(Speed currentSpeed, Speed targetSpeed, double acceleration)
    {
        if (acceleration <= 0 || targetSpeed.MetersPerSecond <= currentSpeed.MetersPerSecond)
            return Time.Zero;

        double timeSeconds = (targetSpeed.MetersPerSecond - currentSpeed.MetersPerSecond) / acceleration;
        return new Time(Math.Max(0, timeSeconds));
    }

    public Distance CalculateAccelerationDistance(Speed currentSpeed, Speed targetSpeed, double acceleration)
    {
        if (acceleration <= 0 || targetSpeed.MetersPerSecond <= currentSpeed.MetersPerSecond)
            return Distance.Zero;

        double distance = (Math.Pow(targetSpeed.MetersPerSecond, 2) - Math.Pow(currentSpeed.MetersPerSecond, 2)) / (2 * acceleration);
        return new Distance(Math.Max(0, distance));
    }
}
