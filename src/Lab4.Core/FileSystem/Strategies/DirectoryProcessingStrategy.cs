using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Nodes;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Strategies;

public class DirectoryProcessingStrategy : INodeProcessingStrategy
{
    public string DisplayPrefix => "[D]";

    public string Type => "Directory";

    public bool HasChildren => true;

    public bool CanHandle(IFileSystemNode node) => Directory.Exists(node.Path);

    public IEnumerable<IFileSystemNode> GetChildren(IFileSystemNode node)
    {
        var children = new List<IFileSystemNode>();

        foreach (string dir in Directory.EnumerateDirectories(node.Path))
        {
            NodeResult result = LocalFileSystemNode.TryCreate(dir);
            if (result is NodeResult.Success success && success.Data is IFileSystemNode childNode)
            {
                children.Add(childNode);
            }
        }

        foreach (string file in Directory.EnumerateFiles(node.Path))
        {
            NodeResult result = LocalFileSystemNode.TryCreate(file);
            if (result is NodeResult.Success success && success.Data is IFileSystemNode childNode)
            {
                children.Add(childNode);
            }
        }

        return children;
    }

    public string ReadContent(IFileSystemNode node)
        => throw new InvalidOperationException("Cannot read content of a directory");
}