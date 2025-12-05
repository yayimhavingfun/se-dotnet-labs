using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Nodes;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem;

public abstract record FileSystemResult
{
    private FileSystemResult() { }

    // successes
    public sealed record Connected(string Address) : FileSystemResult;

    public sealed record Disconnected : FileSystemResult;

    public sealed record DirectoryChanged(string NewPath) : FileSystemResult;

    public sealed record CurrentDirectory(string Path) : FileSystemResult;

    public sealed record FileContent(string Content) : FileSystemResult;

    public sealed record FileMoved : FileSystemResult;

    public sealed record FileCopied : FileSystemResult;

    public sealed record FileDeleted : FileSystemResult;

    public sealed record FileRenamed : FileSystemResult;

    // information results
    public sealed record NodeInfo(IFileSystemNode Node) : FileSystemResult;

    public sealed record ChildrenList(IEnumerable<IFileSystemNode> Children) : FileSystemResult;

    public sealed record PathExists(bool DoesExist) : FileSystemResult;

    public sealed record SpaceAvailable(long AvailableBytes, long TotalBytes) : FileSystemResult;

    public sealed record IsDirectory(bool IsDir) : FileSystemResult;

    public sealed record IsFile(bool IsAFile) : FileSystemResult;

    // errors
    public sealed record NotConnected : FileSystemResult;

    public sealed record NotFound(string Path) : FileSystemResult;

    public sealed record AccessDenied(string Path) : FileSystemResult;

    public sealed record AlreadyConnected : FileSystemResult;

    public sealed record PathOutsideRoot(string Path) : FileSystemResult;

    public sealed record InvalidPath(string Path, string Reason) : FileSystemResult;

    public sealed record OperationFailed(string Operation, string Reason) : FileSystemResult;

    public sealed record InsufficientSpace : FileSystemResult;

    public sealed record NotADirectory(string Path) : FileSystemResult;

    public sealed record NotAFile(string Path) : FileSystemResult;
}