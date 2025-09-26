using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;
using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.RouteSegments;

public abstract class RouteSegment
{
    public Distance Length { get; }

    protected RouteSegment(Distance length)
    {
        ArgumentNullException.ThrowIfNull(length);
        Length = length;
    }

    public abstract RouteSimulationResult Pass(Train train);
}