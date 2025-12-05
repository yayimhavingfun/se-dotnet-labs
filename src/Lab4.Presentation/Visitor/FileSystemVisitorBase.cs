using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Nodes;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Visitor;

public abstract class FileSystemVisitorBase : IFIleSystemVisitor
{
    public abstract void VisitFile(IFileSystemNode fileNode);

    public abstract void VisitDirectory(IFileSystemNode directoryNode, IEnumerable<IFileSystemNode> children);

    public virtual void Visit(IFileSystemNode node, int maxDepth = int.MaxValue)
    {
        ArgumentNullException.ThrowIfNull(node);

        if (maxDepth < 0)
            return;

        if (node.IsDirectory)
        {
            IEnumerable<IFileSystemNode> children = maxDepth > 0 ? node.GetChildren(1) : [];
            VisitDirectory(node, children);

            if (maxDepth > 0)
            {
                foreach (IFileSystemNode child in children)
                {
                    Visit(child, maxDepth - 1);
                }
            }
        }
        else
        {
            VisitFile(node);
        }
    }
}