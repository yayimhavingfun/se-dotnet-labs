using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;

public abstract record RouteSimulationResult
{
    private RouteSimulationResult() { }

    public sealed record Success(Time TotalTime) : RouteSimulationResult;

    public sealed record Failure : RouteSimulationResult;

    public sealed record BrakingFailure : RouteSimulationResult;

    public sealed record AccelerationFailure : RouteSimulationResult;

    public sealed record ForceFailure : RouteSimulationResult;
}
