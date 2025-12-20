using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Nodes;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Strategies;

public class FileProcessingStrategy : INodeProcessingStrategy
{
    public string DisplayPrefix => "[F]";

    public string Type => "File";

    public bool HasChildren => false;

    public bool CanHandle(IFileSystemNode node)
    {
        return !Directory.Exists(node.Path) &&
               File.Exists(node.Path);
    }

    public IEnumerable<IFileSystemNode> GetChildren(IFileSystemNode node)
    {
        return [];
    }

    public string ReadContent(IFileSystemNode node)
    {
        return File.ReadAllText(node.Path);
    }
}