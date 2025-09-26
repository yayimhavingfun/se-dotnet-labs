using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;

public abstract record TrainOperationResult
{
    private TrainOperationResult() { }

    public sealed record Success(Time TotalTime) : TrainOperationResult;

    public sealed record Failure : TrainOperationResult;
}
