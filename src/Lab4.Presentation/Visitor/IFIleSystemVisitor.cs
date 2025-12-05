using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Nodes;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Visitor;

public interface IFIleSystemVisitor
{
    void VisitFile(IFileSystemNode fileNode);

    void VisitDirectory(IFileSystemNode directoryNode, IEnumerable<IFileSystemNode> children);

    void Visit(IFileSystemNode node, int maxDepth = int.MaxValue);
}