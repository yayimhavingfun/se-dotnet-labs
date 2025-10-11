using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;
using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Services;

public class MovementCalculator
{
    private readonly Time _precision;

    public MovementCalculator(Time precision)
    {
        ArgumentNullException.ThrowIfNull(precision);

        _precision = precision;
    }

    public MovementResult CalculateStep(double currentSpeed, double acceleration, double remainingDistance)
    {
        double resultantSpeed = currentSpeed + (acceleration * _precision.Seconds);

        if (resultantSpeed <= 0)
            return new MovementResult.Failure("Speed became negative or zero");

        double distanceCovered = resultantSpeed * _precision.Seconds;

        if (distanceCovered >= remainingDistance)
        {
            double exactTime = remainingDistance / resultantSpeed;
            return new MovementResult.Success(new Time(exactTime), new Speed(resultantSpeed));
        }

        return new MovementResult.ContinueMovement(
            new Time(_precision.Seconds),
            new Speed(resultantSpeed),
            remainingDistance - distanceCovered);
    }
}
