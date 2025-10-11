using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;
using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Services;

public class MovementHelper
{
    private readonly MovementCalculator _movementCalculator;
    private readonly Simulator _simulator;

    public MovementHelper(MovementCalculator movementCalculator, Simulator simulator)
    {
        _movementCalculator = movementCalculator;
        _simulator = simulator;
    }

    public TrainOperationResult AccelerateToSpeed(Train train, Speed targetSpeed)
    {
        ArgumentNullException.ThrowIfNull(targetSpeed);

        if (targetSpeed.MetersPerSecond < 0)
            return new TrainOperationResult.Failure();

        if (Math.Abs(train.Speed.MetersPerSecond - targetSpeed.MetersPerSecond) < 0.001)
            return new TrainOperationResult.Success(Time.Zero);

        var requiredForce = new Force(Math.Abs(train.MaxForce.Newtons * 0.7));
        if (!train.TryApplyForce(requiredForce))
            return new TrainOperationResult.Failure();

        double acceleration = _simulator.CalculateAcceleration(requiredForce, train.Mass);
        Time timeToReach = _simulator.CalculateTimeToReachSpeed(train.Speed, targetSpeed, acceleration);
        if (timeToReach.Seconds <= 0)
            return new TrainOperationResult.Failure();

        Distance accelerationDistance = _simulator.CalculateAccelerationDistance(train.Speed, targetSpeed, acceleration);
        TrainOperationResult passResult = train.PassDistance(accelerationDistance);

        if (passResult is TrainOperationResult.Success success)
        {
            train.Speed = targetSpeed;
            return new TrainOperationResult.Success(success.TotalTime);
        }

        return passResult;
    }

    public TrainOperationResult BrakeToStop(Train train)
    {
        if (Math.Abs(train.Speed.MetersPerSecond) < 0.001)
            return new TrainOperationResult.Success(Time.Zero);

        Time timeToStop = _simulator.CalculateTimeToStop(train.Speed, train.Acceleration);
        if (timeToStop.Seconds < 0)
            return new TrainOperationResult.Failure();

        double deceleration = -train.Speed.MetersPerSecond / timeToStop.Seconds;

        var requiredForce = new Force(-Math.Abs(train.MaxForce.Newtons * 0.8));

        if (!train.TryApplyForce(requiredForce))
            return new TrainOperationResult.Failure();

        var brakingDistance = new Distance(_simulator.CalculateBrakingDistance(train.Speed, deceleration));
        Time brakingTime = _simulator.CalculateTimeToStop(train.Speed, deceleration);

        train.Speed = Speed.Zero;
        train.Acceleration = 0;

        return new TrainOperationResult.Success(brakingTime);
    }

    public TrainOperationResult CalculateMovement(Train train, Distance distance)
    {
        double remainingDistance = distance.Meters;
        Time totalTime = Time.Zero;
        double currentSpeed = train.Speed.MetersPerSecond;

        while (remainingDistance > 0)
        {
            MovementResult result = _movementCalculator.CalculateStep(currentSpeed, train.Acceleration, remainingDistance);

            switch (result)
            {
                case MovementResult.Success success:
                    train.Speed = new Speed(success.FinalSpeed.MetersPerSecond);
                    return new TrainOperationResult.Success(totalTime.Add(success.Time));

                case MovementResult.ContinueMovement cont:
                    totalTime = totalTime.Add(cont.Time);
                    currentSpeed = cont.NewSpeed.MetersPerSecond;
                    remainingDistance = cont.RemainingDistance;
                    break;

                case MovementResult.Failure:
                    return new TrainOperationResult.Failure();

                default:
                    return new TrainOperationResult.Failure();
            }
        }

        train.Speed = new Speed(currentSpeed);
        return new TrainOperationResult.Success(totalTime);
    }
}
