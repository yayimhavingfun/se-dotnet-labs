using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;
using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.RouteSegments;

public class PowerSegment : IRouteSegment
{
    private readonly Distance _length;
    private readonly Force _force;

    public PowerSegment(Distance length, Force force)
    {
        ArgumentNullException.ThrowIfNull(length);
        ArgumentNullException.ThrowIfNull(force);

        _length = length;
        _force = force;
    }

    public RouteSimulationResult Pass(Train train)
    {
        ArgumentNullException.ThrowIfNull(train);

        if (train.ApplyForce(_force) == false)
            return new RouteSimulationResult.ForceFailure();

        return train.PassDistance(_length) switch
        {
            TrainOperationResult.Success success => new RouteSimulationResult.Success(success.TotalTime),
            _ => new RouteSimulationResult.Failure(),
        };
    }
}
