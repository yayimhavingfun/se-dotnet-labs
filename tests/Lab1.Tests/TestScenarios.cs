using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;
using Itmo.ObjectOrientedProgramming.Lab1.RouteSegments;
using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public class TestScenarios
{
    [Fact]
    public void Scenario1_ShouldReturnSuccess()
    {
        Train train = CreateDefaultTrain();
        var route = new Route(new Speed(30));

        route.AddSegment(new PowerSegment(new Distance(50), new Force(1000)));
        route.AddSegment(new RegularSegment(new Distance(20)));

        RouteSimulationResult result = route.Simulate(train);

        Assert.IsType<RouteSimulationResult.Success>(result);

        var successResult = (RouteSimulationResult.Success)result;
        Assert.True(train.Speed.MetersPerSecond <= 25);
    }

    [Fact]
    public void Scenario2_ShouldReturnFailure()
    {
        Train train = CreateDefaultTrain();
        var route = new Route(new Speed(5));

        route.AddSegment(new PowerSegment(new Distance(100), new Force(4000)));
        route.AddSegment(new RegularSegment(new Distance(50)));

        RouteSimulationResult result = route.Simulate(train);

        Assert.IsType<RouteSimulationResult.Failure>(result);
    }

    [Fact]
    public void Scenario3_ShouldReturnSuccess()
    {
        Train train = CreateDefaultTrain();

        var route = new Route(new Speed(30));

        Console.WriteLine($"Initial train state: Speed = {train.Speed.MetersPerSecond}, Acceleration = {train.Acceleration}");

        route.AddSegment(new PowerSegment(new Distance(80), new Force(800))); // speed = 10
        route.AddSegment(new RegularSegment(new Distance(30)));
        route.AddSegment(new StationSegment(new Speed(15), new Time(45)));
        route.AddSegment(new RegularSegment(new Distance(40)));

        RouteSimulationResult result = route.Simulate(train);

        Assert.IsType<RouteSimulationResult.Success>(result);
    }

    [Fact]
    public void Scenario4_ShouldReturnFailure()
    {
        Train train = CreateDefaultTrain();
        var route = new Route(new Speed(30));

        route.AddSegment(new PowerSegment(new Distance(100), new Force(3000)));
        route.AddSegment(new StationSegment(new Speed(5), new Time(30)));
        route.AddSegment(new RegularSegment(new Distance(50)));

        RouteSimulationResult result = route.Simulate(train);

        Assert.IsType<RouteSimulationResult.Failure>(result);
    }

    [Fact]
    public void Scenario5_ShouldReturnFailure()
    {
        Train train = CreateDefaultTrain();
        var route = new Route(new Speed(15));

        route.AddSegment(new PowerSegment(new Distance(100), new Force(2500)));
        route.AddSegment(new RegularSegment(new Distance(40)));
        route.AddSegment(new StationSegment(new Speed(25), new Time(45)));
        route.AddSegment(new RegularSegment(new Distance(60)));

        RouteSimulationResult result = route.Simulate(train);

        Assert.IsType<RouteSimulationResult.Failure>(result);
    }

    [Fact]
    public void Scenario6_ShouldReturnSuccess()
    {
        Train train = CreateDefaultTrain();
        var route = new Route(new Speed(20));

        route.AddSegment(new PowerSegment(new Distance(50), new Force(1200)));
        route.AddSegment(new RegularSegment(new Distance(15)));
        route.AddSegment(new PowerSegment(new Distance(35), new Force(-1000)));
        route.AddSegment(new StationSegment(new Speed(10), new Time(45)));
        route.AddSegment(new RegularSegment(new Distance(25)));
        route.AddSegment(new PowerSegment(new Distance(60), new Force(1100)));
        route.AddSegment(new RegularSegment(new Distance(20)));
        route.AddSegment(new PowerSegment(new Distance(40), new Force(-1200)));

        RouteSimulationResult result = route.Simulate(train);

        Assert.IsType<RouteSimulationResult.Success>(result);
    }

    [Fact]
    public void Scenario7_ShouldReturnFailure()
    {
        Train train = CreateDefaultTrain();
        var route = new Route(new Speed(30));

        route.AddSegment(new RegularSegment(new Distance(100)));

        RouteSimulationResult result = route.Simulate(train);

        Assert.IsType<RouteSimulationResult.Failure>(result);
    }

    [Fact]
    public void Scenario8_ShouldReturnFailure()
    {
        Train train = CreateDefaultTrain();
        var route = new Route(new Speed(30));

        const double x = 50;
        const double y = 2000;

        route.AddSegment(new PowerSegment(new Distance(x), new Force(y)));
        route.AddSegment(new PowerSegment(new Distance(x), new Force(-2 * y)));

        RouteSimulationResult result = route.Simulate(train);

        Assert.IsType<RouteSimulationResult.Failure>(result);
    }

    private Train CreateDefaultTrain()
    {
        return new Train(
            mass: 1000,
            maxForce: new Force(5000),
            precision: new Time(0.1));
    }
}
