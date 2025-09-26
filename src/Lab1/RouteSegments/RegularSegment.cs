using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;
using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.RouteSegments;

public class RegularSegment : RouteSegment
{
    public RegularSegment(Distance length) : base(length) { }

    public override RouteSimulationResult Pass(Train train)
    {
        ArgumentNullException.ThrowIfNull(train);

        return train.PassDistance(Length) switch
        {
            TrainOperationResult.Success success => new RouteSimulationResult.Success(success.TotalTime),
            _ => new RouteSimulationResult.Failure(),
        };
    }
}
