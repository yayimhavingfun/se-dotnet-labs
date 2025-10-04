using Itmo.ObjectOrientedProgramming.Lab1.Entities;
using Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;
using Itmo.ObjectOrientedProgramming.Lab1.RouteSegments;
using Itmo.ObjectOrientedProgramming.Lab1.Services;
using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab1.Tests;

public class RouteSimulationTests
{
    private readonly MovementHelper _movementHelper;

    public RouteSimulationTests()
    {
        var simulator = new Simulator();
        var movementCalculator = new MovementCalculator(new Time(0.1));
        _movementHelper = new MovementHelper(movementCalculator, simulator);
    }

    [Fact]
    public void Simulate_ShouldReturnSuccess_WhenPowerPathAcceleratesWithinRouteLimit()
    {
        // Arrange
        Train train = CreateDefaultTrain();
        var segments = new IRouteSegment[]
        {
            new PowerSegment(new Distance(50), new Force(1000)),
            new RegularSegment(new Distance(20)),
        };
        var route = new Route(new Speed(30), segments);

        // Act
        RouteSimulationResult result = route.Simulate(train);

        // Assert
        Assert.IsType<RouteSimulationResult.Success>(result);
    }

    [Fact]
    public void Simulate_ShouldReturnFailure_WhenPowerPathExceedsRouteSpeedLimit()
    {
        // Arrange
        Train train = CreateDefaultTrain();
        var segments = new IRouteSegment[]
        {
        new PowerSegment(new Distance(100), new Force(4000)),
        new RegularSegment(new Distance(50)),
        };
        var route = new Route(new Speed(5), segments);

        // Act
        RouteSimulationResult result = route.Simulate(train);

        // Assert
        Assert.IsType<RouteSimulationResult.Failure>(result);
    }

    [Fact]
    public void Simulate_ShouldReturnSuccess_WhenTrainSpeedIsWithinBothRouteAndStationLimits()
    {
        // Arrange
        Train train = CreateDefaultTrain();
        var segments = new IRouteSegment[]
        {
            new PowerSegment(new Distance(80), new Force(800)), // speed = 10
            new RegularSegment(new Distance(30)),
            new StationSegment(new Speed(15), new Time(45), _movementHelper),
            new RegularSegment(new Distance(40)),
        };
        var route = new Route(new Speed(30), segments);

        // Act
        RouteSimulationResult result = route.Simulate(train);

        // Assert
        Assert.IsType<RouteSimulationResult.Success>(result);
    }

    [Fact]
    public void Simulate_ShouldReturnFailure_WhenTrainExceedsStationSpeedLimit()
    {
        // Arrange
        Train train = CreateDefaultTrain();
        var segments = new IRouteSegment[]
        {
            new PowerSegment(new Distance(100), new Force(3000)),
            new StationSegment(new Speed(5), new Time(30), _movementHelper),
            new RegularSegment(new Distance(50)),
        };
        var route = new Route(new Speed(30), segments);

        // Act
        RouteSimulationResult result = route.Simulate(train);

        // Assert
        Assert.IsType<RouteSimulationResult.Failure>(result);
    }

    [Fact]
    public void Simulate_ShouldReturnFailure_WhenTrainExceedsRouteLimitButNotStationLimit()
    {
        // Arrange
        Train train = CreateDefaultTrain();
        var segments = new IRouteSegment[]
         {
            new PowerSegment(new Distance(100), new Force(2500)),
            new RegularSegment(new Distance(40)),
            new StationSegment(new Speed(25), new Time(45), _movementHelper),
            new RegularSegment(new Distance(60)),
         };
        var route = new Route(new Speed(15), segments);

        // Act
        RouteSimulationResult result = route.Simulate(train);

        // Assert
        Assert.IsType<RouteSimulationResult.Failure>(result);
    }

    [Fact]
    public void Simulate_ShouldReturnSuccess_WhenComplexRouteWithAccelerationAndBrakingIsWithinLimits()
    {
        // Arrange
        Train train = CreateDefaultTrain();
        var segments = new IRouteSegment[]
        {
            new PowerSegment(new Distance(50), new Force(1200)),
            new RegularSegment(new Distance(15)),
            new PowerSegment(new Distance(35), new Force(-1000)),
            new StationSegment(new Speed(10), new Time(45), _movementHelper),
            new RegularSegment(new Distance(25)),
            new PowerSegment(new Distance(60), new Force(1100)),
            new RegularSegment(new Distance(20)),
            new PowerSegment(new Distance(40), new Force(-1200)),
        };
        var route = new Route(new Speed(20), segments);

        // Act
        RouteSimulationResult result = route.Simulate(train);

        // Assert
        Assert.IsType<RouteSimulationResult.Success>(result);
    }

    [Fact]
    public void Simulate_ShouldReturnFailure_WhenTrainCannotMoveOnRegularPathWithoutInitialMotion()
    {
        // Arrange
        Train train = CreateDefaultTrain();
        var segments = new IRouteSegment[]
        {
            new RegularSegment(new Distance(100)),
        };
        var route = new Route(new Speed(30), segments);

        // Act
        RouteSimulationResult result = route.Simulate(train);

        // Assert
        Assert.IsType<RouteSimulationResult.Failure>(result);
    }

    [Fact]
    public void Simulate_ShouldReturnFailure_WhenBrakingForceExceedsTrainCapabilities()
    {
        // Arrange
        Train train = CreateDefaultTrain();
        const double x = 50;
        const double y = 2000;
        var segments = new IRouteSegment[]
        {
            new PowerSegment(new Distance(x), new Force(y)),
            new PowerSegment(new Distance(x), new Force(-2 * y)),
        };
        var route = new Route(new Speed(30), segments);

        // Act
        RouteSimulationResult result = route.Simulate(train);

        // Assert
        Assert.IsType<RouteSimulationResult.Failure>(result);
    }

    private Train CreateDefaultTrain()
    {
        var simulator = new Simulator();
        return new Train(
            mass: new Mass(1000),
            maxForce: new Force(5000),
            precision: new Time(0.1),
            simulator: simulator);
    }
}