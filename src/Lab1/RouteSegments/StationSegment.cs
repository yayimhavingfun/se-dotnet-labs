using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;
using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.RouteSegments;

public class StationSegment : RouteSegment
{
    public Speed MaxAllowedSpeed { get; }

    public Time BaseBoardingTime { get; }

    public double CongestionCoefficient { get; }

    public StationSegment(Speed maxAllowedSpeed, Time baseBoardingTime, double congestionCoefficient = 1.0) : base(Distance.Zero)
    {
        if (maxAllowedSpeed.MetersPerSecond < 0)
            throw new ArgumentException("Max allowed speed cannot be negative");

        ArgumentNullException.ThrowIfNull(maxAllowedSpeed);
        ArgumentNullException.ThrowIfNull(baseBoardingTime);

        MaxAllowedSpeed = maxAllowedSpeed;
        BaseBoardingTime = baseBoardingTime;

        CongestionCoefficient = Math.Max(0.1, Math.Min(congestionCoefficient, 5.0));
    }

    public override RouteSimulationResult Pass(Train train)
    {
        ArgumentNullException.ThrowIfNull(train);

        if (train.Speed.MetersPerSecond > MaxAllowedSpeed.MetersPerSecond)
            return new RouteSimulationResult.Failure();

        Speed arrivalSpeed = train.Speed;
        Time totalTime = Time.Zero;

        TrainOperationResult brakingResult = train.BrakeToStop();

        if (brakingResult is TrainOperationResult.Failure)
            return new RouteSimulationResult.BrakingFailure();

        if (brakingResult is TrainOperationResult.Success brakingSuccess)
            totalTime = totalTime.Add(brakingSuccess.TotalTime);

        var actualBoardingTime = new Time(BaseBoardingTime.Seconds * CongestionCoefficient);
        totalTime = totalTime.Add(actualBoardingTime);

        TrainOperationResult accelerationResult = train.AccelerateToSpeed(arrivalSpeed);

        if (accelerationResult is TrainOperationResult.Failure)
            return new RouteSimulationResult.AccelerationFailure();

        if (accelerationResult is TrainOperationResult.Success accelerationSuccess)
            totalTime = totalTime.Add(accelerationSuccess.TotalTime);

        return new RouteSimulationResult.Success(totalTime);
    }
}
