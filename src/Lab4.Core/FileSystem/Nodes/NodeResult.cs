namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Nodes;

public abstract record NodeResult
{
    private NodeResult() { }

    public sealed record NodeCreated(IFileSystemNode Node) : NodeResult;

    public sealed record ChildrenLoaded(IEnumerable<IFileSystemNode> Children) : NodeResult;

    public sealed record AccessDenied(string Path) : NodeResult;

    public sealed record NotFound(string Path) : NodeResult;

    public sealed record IoError(string Path, string Message) : NodeResult;

    public bool IsSuccess => this is NodeCreated or ChildrenLoaded;

    public bool IsFailure => !IsSuccess;

    public IFileSystemNode? GetNodeOrNull() => this switch
    {
        NodeCreated created => created.Node,
        _ => null,
    };

    public IEnumerable<IFileSystemNode>? GetChildrenOrNull() => this switch
    {
        ChildrenLoaded loaded => loaded.Children,
        _ => null,
    };
}