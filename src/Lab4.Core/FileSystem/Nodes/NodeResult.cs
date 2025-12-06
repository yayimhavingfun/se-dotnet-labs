namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Nodes;

public abstract record NodeResult
{
    private NodeResult() { }

    public sealed record Success(object? Data = null) : NodeResult;

    public sealed record Failure(string ErrorCode, string Message, string? Path = null) : NodeResult;
}