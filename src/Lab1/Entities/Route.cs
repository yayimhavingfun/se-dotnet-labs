using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;
using Itmo.ObjectOrientedProgramming.Lab1.RouteSegments;
using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Entities;

public class Route
{
    private readonly ICollection<IRouteSegment> _segments;
    private readonly Speed _maxFinalSpeed;

    public Route(Speed maxFinalSpeed, ICollection<IRouteSegment> segments)
    {
        if (maxFinalSpeed.MetersPerSecond < 0)
            throw new ArgumentException("Max final speed cannot be negative.");

        ArgumentNullException.ThrowIfNull(segments);

        _segments = segments;
        _maxFinalSpeed = maxFinalSpeed;
    }

    public RouteSimulationResult Simulate(Train train)
    {
        ArgumentNullException.ThrowIfNull(train);

        double totalTime = 0.0;

        foreach (IRouteSegment segment in _segments)
        {
            RouteSimulationResult segmentResult = segment.Pass(train);

            if (segmentResult is RouteSimulationResult.Success success)
                totalTime += success.TotalTime.Seconds;
            else
                return segmentResult;
        }

        if (train.Speed.MetersPerSecond > _maxFinalSpeed.MetersPerSecond)
            return new RouteSimulationResult.Failure();

        return new RouteSimulationResult.Success(new Time(totalTime));
    }
}