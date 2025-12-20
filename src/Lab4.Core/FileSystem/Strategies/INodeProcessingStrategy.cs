using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Nodes;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Strategies;

public interface INodeProcessingStrategy
{
    bool CanHandle(IFileSystemNode node);

    string DisplayPrefix { get; }

    string Type { get; }

    bool HasChildren { get; }

    IEnumerable<IFileSystemNode> GetChildren(IFileSystemNode node);

    string ReadContent(IFileSystemNode node);
}