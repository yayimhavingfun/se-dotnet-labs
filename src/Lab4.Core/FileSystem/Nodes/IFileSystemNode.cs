namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Nodes;

public interface IFileSystemNode
{
    string Name { get; }

    string Path { get; }

    bool IsDirectory { get; }

    long Size { get; }

    DateTime LastModified { get; }

    IEnumerable<IFileSystemNode> GetChildren(int maxDepth = 1);
}