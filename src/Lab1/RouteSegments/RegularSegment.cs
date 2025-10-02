using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;
using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.RouteSegments;

public class RegularSegment : IRouteSegment
{
    private readonly Distance _length;

    public RegularSegment(Distance length)
    {
        ArgumentNullException.ThrowIfNull(length);

        _length = length;
    }

    public RouteSimulationResult Pass(Train train)
    {
        ArgumentNullException.ThrowIfNull(train);

        return train.PassDistance(_length) switch
        {
            TrainOperationResult.Success success =>
            new RouteSimulationResult.Success(success.TotalTime),
            _ => new RouteSimulationResult.Failure(),
        };
    }
}
