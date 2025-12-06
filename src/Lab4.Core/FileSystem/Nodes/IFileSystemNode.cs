using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Strategies;
using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Visitor;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Nodes;

public interface IFileSystemNode
{
    string Name { get; }

    string Type { get; }

    string Path { get; }

    long Size { get; }

    DateTime LastModified { get; }

    INodeProcessingStrategy Strategy { get; }

    IEnumerable<IFileSystemNode> GetChildren(int maxDepth = 1);

    void Accept(IFileSystemVisitor visitor);
}