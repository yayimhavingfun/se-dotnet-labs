using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;

namespace Itmo.ObjectOrientedProgramming.Lab1.RouteSegments;

public interface IRouteSegment
{
    RouteSimulationResult Pass(Train train);
}