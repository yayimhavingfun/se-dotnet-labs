using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;
using Itmo.ObjectOrientedProgramming.Lab1.Services;
using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Entities;

public class Train
{
    private readonly Force _maxForce;
    private readonly MovementCalculator _movementCalculator;
    private readonly double _mass;

    private double _acceleration;

    public Speed Speed { get; set; }

    public Train(double mass, Force maxForce, Time precision)
    {
        if (mass <= 0) throw new ArgumentException("Mass must be positive");
        ArgumentNullException.ThrowIfNull(maxForce);
        ArgumentNullException.ThrowIfNull(precision);

        _mass = mass;
        _maxForce = maxForce;
        Speed = Speed.Zero;
        _acceleration = 0;
        _movementCalculator = new MovementCalculator(precision);
    }

    public bool ApplyForce(Force force)
    {
        if (!PhysicsService.CanApplyForce(force.Newtons, _maxForce.Newtons))
            return false;

        _acceleration = PhysicsService.CalculateAcceleration(force, _mass);
        return true;
    }

    public TrainOperationResult PassDistance(Distance distance)
    {
        ArgumentNullException.ThrowIfNull(distance);

        if (Math.Abs(Speed.MetersPerSecond) < 0.001 && _acceleration <= 0)
            return new TrainOperationResult.Failure();

        double remainingDistance = distance.Meters;
        Time totalTime = Time.Zero;
        double currentSpeed = Speed.MetersPerSecond;

        while (remainingDistance > 0)
        {
            MovementResult result = _movementCalculator.CalculateStep(currentSpeed, _acceleration, remainingDistance);

            switch (result)
            {
                case MovementResult.Success success:
                    Speed = new Speed(success.FinalSpeed.MetersPerSecond);
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

        Speed = new Speed(currentSpeed);

        return new TrainOperationResult.Success(totalTime);
    }

    public TrainOperationResult AccelerateToSpeed(Speed targetSpeed)
    {
        ArgumentNullException.ThrowIfNull(targetSpeed);

        if (targetSpeed.MetersPerSecond < 0)
            return new TrainOperationResult.Failure();

        if (Math.Abs(Speed.MetersPerSecond - targetSpeed.MetersPerSecond) < 0.001)
            return new TrainOperationResult.Success(Time.Zero);

        var requiredForce = new Force(Math.Abs(_maxForce.Newtons * 0.7));
        if (!ApplyForce(requiredForce))
            return new TrainOperationResult.Failure();

        double acceleration = PhysicsService.CalculateAcceleration(requiredForce, _mass);
        Time timeToReach = PhysicsService.CalculateTimeToReachSpeed(Speed, targetSpeed, acceleration);
        if (timeToReach.Seconds <= 0)
            return new TrainOperationResult.Failure();

        Distance accelerationDistance = PhysicsService.CalculateAccelerationDistance(Speed, targetSpeed, _acceleration);
        TrainOperationResult passResult = PassDistance(accelerationDistance);

        if (passResult is TrainOperationResult.Success success)
        {
            Speed = targetSpeed;
            return new TrainOperationResult.Success(success.TotalTime);
        }

        return passResult;
    }

    public TrainOperationResult BrakeToStop()
    {
        if (Math.Abs(Speed.MetersPerSecond) < 0.001)
            return new TrainOperationResult.Success(Time.Zero);

        Time timeToStop = PhysicsService.CalculateTimeToStop(Speed, _acceleration);
        if (timeToStop.Seconds < 0)
            return new TrainOperationResult.Failure();

        double deceleration = -Speed.MetersPerSecond / timeToStop.Seconds;

        var requiredForce = new Force(-Math.Abs(_maxForce.Newtons * 0.8));

        if (!ApplyForce(requiredForce))
            return new TrainOperationResult.Failure();

        var brakingDistance = new Distance(PhysicsService.CalculateBrakingDistance(Speed, deceleration));
        Time brakingTime = PhysicsService.CalculateTimeToStop(Speed, deceleration);

        Speed = Speed.Zero;
        _acceleration = 0;

        return new TrainOperationResult.Success(brakingTime);
    }

    public bool ExceedsSpeedLimit(Speed maxAllowedSpeed)
    {
        return Speed.MetersPerSecond > maxAllowedSpeed.MetersPerSecond;
    }
}
