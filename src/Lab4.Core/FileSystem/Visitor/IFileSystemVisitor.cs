using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Nodes;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Visitor;

public interface IFileSystemVisitor
{
    void Visit(IFileSystemNode node, int maxDepth = 1);
}