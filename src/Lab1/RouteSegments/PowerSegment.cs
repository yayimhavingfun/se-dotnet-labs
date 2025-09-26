using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;
using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.RouteSegments;

public class PowerSegment : RouteSegment
{
    public Force Force { get; }

    public PowerSegment(Distance length, Force force) : base(length)
    {
        Force = force;
    }

    public override RouteSimulationResult Pass(Train train)
    {
        ArgumentNullException.ThrowIfNull(train);

        if (train.ApplyForce(Force) == false)
            return new RouteSimulationResult.ForceFailure();

        return train.PassDistance(Length) switch
        {
            TrainOperationResult.Success success => new RouteSimulationResult.Success(success.TotalTime),
            _ => new RouteSimulationResult.Failure(),
        };
    }
}
