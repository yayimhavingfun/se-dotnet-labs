using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;
using Itmo.ObjectOrientedProgramming.Lab1.Services;
using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Entities;

public class Train
{
    private readonly MovementService _movementService;

    public Speed Speed { get; set; }

    public Force MaxForce { get; }

    public double Mass { get; }

    public double Acceleration { get; set; }

    public Train(double mass, Force maxForce, Time precision)
    {
        if (mass <= 0) throw new ArgumentException("Mass must be positive");
        ArgumentNullException.ThrowIfNull(maxForce);
        ArgumentNullException.ThrowIfNull(precision);

        Mass = mass;
        MaxForce = maxForce;
        Speed = Speed.Zero;
        Acceleration = 0;
        var movementCalculator = new MovementCalculator(precision);
        _movementService = new MovementService(movementCalculator);
    }

    public bool ApplyForce(Force force)
    {
        if (!PhysicsService.CanApplyForce(force.Newtons, MaxForce.Newtons))
            return false;

        Acceleration = PhysicsService.CalculateAcceleration(force, Mass);
        return true;
    }

    public TrainOperationResult PassDistance(Distance distance)
    {
        ArgumentNullException.ThrowIfNull(distance);

        if (Math.Abs(Speed.MetersPerSecond) < 0.001 && Acceleration <= 0)
            return new TrainOperationResult.Failure();

        return _movementService.CalculateMovement(this, distance);
    }
}
