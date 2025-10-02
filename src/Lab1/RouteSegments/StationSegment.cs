using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;
using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.RouteSegments;

public class StationSegment : IRouteSegment
{
    private readonly Speed _maxAllowedSpeed;
    private readonly Time _baseBoardingTime;
    private readonly double _congestionCoefficient;

    public StationSegment(Speed maxAllowedSpeed, Time baseBoardingTime, double congestionCoefficient = 1.0)
    {
        ArgumentNullException.ThrowIfNull(maxAllowedSpeed);
        ArgumentNullException.ThrowIfNull(baseBoardingTime);

        if (maxAllowedSpeed.MetersPerSecond < 0)
            throw new ArgumentException("Max allowed speed cannot be negative");

        _maxAllowedSpeed = maxAllowedSpeed;
        _baseBoardingTime = baseBoardingTime;

        _congestionCoefficient = Math.Max(0.1, Math.Min(congestionCoefficient, 5.0));
    }

    public RouteSimulationResult Pass(Train train)
    {
        ArgumentNullException.ThrowIfNull(train);

        if (train.ExceedsSpeedLimit(_maxAllowedSpeed))
            return new RouteSimulationResult.Failure();

        Speed arrivalSpeed = train.Speed;
        Time totalTime = Time.Zero;

        TrainOperationResult brakingResult = train.BrakeToStop();
        if (brakingResult is not TrainOperationResult.Success brakingSuccess)
            return new RouteSimulationResult.BrakingFailure();

        var actualBoardingTime = new Time(_baseBoardingTime.Seconds * _congestionCoefficient);
        totalTime = totalTime.Add(actualBoardingTime);

        TrainOperationResult accelerationResult = train.AccelerateToSpeed(_maxAllowedSpeed);
        if (accelerationResult is not TrainOperationResult.Success accelerationSuccess)
            return new RouteSimulationResult.AccelerationFailure();

        totalTime = totalTime.Add(accelerationSuccess.TotalTime);
        return new RouteSimulationResult.Success(new Time(_baseBoardingTime.Seconds * _congestionCoefficient));
    }
}
