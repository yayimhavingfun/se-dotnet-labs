using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;
using Itmo.ObjectOrientedProgramming.Lab1.Services;
using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Entities;

public class Train
{
    private readonly Simulator _simulator;
    private readonly MovementHelper _movementHelper;

    public Speed Speed { get; set; }

    public Force MaxForce { get; }

    public Mass Mass { get; }

    public double Acceleration { get; set; }

    public Train(Mass mass, Force maxForce, Time precision, Simulator simulator)
    {
        ArgumentNullException.ThrowIfNull(mass);
        ArgumentNullException.ThrowIfNull(maxForce);
        ArgumentNullException.ThrowIfNull(precision);

        Mass = mass;
        MaxForce = maxForce;
        Speed = Speed.Zero;
        Acceleration = 0;
        _simulator = simulator;
        var movementCalculator = new MovementCalculator(precision);
        _movementHelper = new MovementHelper(movementCalculator, simulator);
    }

    public bool TryApplyForce(Force force)
    {
        if (!_simulator.CanApplyForce(force.Newtons, MaxForce.Newtons))
            return false;

        Acceleration = _simulator.CalculateAcceleration(force, Mass);
        return true;
    }

    public TrainOperationResult PassDistance(Distance distance)
    {
        ArgumentNullException.ThrowIfNull(distance);

        if (Math.Abs(Speed.MetersPerSecond) < 0.001 && Acceleration <= 0)
            return new TrainOperationResult.Failure();

        return _movementHelper.CalculateMovement(this, distance);
    }
}
