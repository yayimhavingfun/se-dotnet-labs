using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;
using Itmo.ObjectOrientedProgramming.Lab1.RouteSegments;
using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.Entities;

public class Route
{
    private readonly List<RouteSegment> _segments;

    public Speed MaxFinalSpeed { get; }

    public Route(Speed maxFinalSpeed)
    {
        if (maxFinalSpeed.MetersPerSecond < 0) throw new ArgumentException("Max final speed cannot be negative.");

        _segments = new List<RouteSegment>();
        MaxFinalSpeed = maxFinalSpeed;
    }

    public void AddSegment(RouteSegment segment)
    {
        ArgumentNullException.ThrowIfNull(segment);

        _segments.Add(segment);
    }

    public RouteSimulationResult Simulate(Train train)
    {
        ArgumentNullException.ThrowIfNull(train);

        double totalTime = 0.0;

        foreach (RouteSegment segment in _segments)
        {
            RouteSimulationResult segmentResult = segment.Pass(train);

            if (segmentResult is RouteSimulationResult.Success success)
                totalTime += success.TotalTime.Seconds;
            else
                return segmentResult;
        }

        if (train.Speed.MetersPerSecond > MaxFinalSpeed.MetersPerSecond)
            return new RouteSimulationResult.Failure();

        return new RouteSimulationResult.Success(new Time(totalTime));
    }
}