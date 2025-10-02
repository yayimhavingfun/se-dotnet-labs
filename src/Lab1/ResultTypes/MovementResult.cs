using Itmo.ObjectOrientedProgramming.Lab1.ValueObjects;

namespace Itmo.ObjectOrientedProgramming.Lab1.ResultTypes;

public abstract record MovementResult
{
    private MovementResult() { }

    public sealed record Success(Time Time, Speed FinalSpeed) : MovementResult;

    public sealed record ContinueMovement(Time Time, Speed NewSpeed, double RemainingDistance) : MovementResult;

    public sealed record Failure(string Reason) : MovementResult;
}
