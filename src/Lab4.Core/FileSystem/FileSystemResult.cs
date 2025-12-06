namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem;

public abstract record FileSystemResult
{
    private FileSystemResult() { }

    public sealed record Success(string Operation, string Comment, object? Data = null) : FileSystemResult;

    public sealed record Failure(string Operation, string Reason) : FileSystemResult;
}