using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Strategies;
using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Visitor;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Nodes;

public class LocalFileSystemNode : IFileSystemNode
{
    private readonly Lazy<NodeResult> _children;
    private readonly Lazy<FileSystemInfo> _info;

    public string Path { get; }

    public string Name => _info.Value.Name;

    public string Type => Strategy.Type;

    public INodeProcessingStrategy Strategy { get; }

    public long Size => Strategy is FileProcessingStrategy ? ((FileInfo)_info.Value).Length : 0;

    public DateTime LastModified => _info.Value.LastWriteTime;

    public LocalFileSystemNode(string fullPath)
    {
        if (string.IsNullOrWhiteSpace(fullPath))
            throw new ArgumentException("Path cannot be null or empty", nameof(fullPath));

        Path = fullPath;

        bool pathExists = File.Exists(fullPath) || Directory.Exists(fullPath);
        if (!pathExists)
            throw new FileNotFoundException($"Path not found: {fullPath}", fullPath);

        _info = new Lazy<FileSystemInfo>(() =>
        {
            if (Directory.Exists(fullPath))
                return new DirectoryInfo(fullPath);

            return new FileInfo(fullPath);
        });

        Strategy = Directory.Exists(fullPath)
            ? new DirectoryProcessingStrategy()
            : new FileProcessingStrategy();

        _children = new Lazy<NodeResult>(() =>
        {
            return Directory.Exists(fullPath)
                ? LoadChildren()
                : new NodeResult.Success(new List<IFileSystemNode>());
        });
    }

    public static NodeResult TryCreate(string fullPath)
    {
        try
        {
            var node = new LocalFileSystemNode(fullPath);
            return new NodeResult.Success(node);
        }
        catch (UnauthorizedAccessException)
        {
            return new NodeResult.Failure("ACCESS_DENIED", "Access denied", fullPath);
        }
        catch (FileNotFoundException)
        {
            return new NodeResult.Failure("NOT_FOUND", "Path not found", fullPath);
        }
        catch (IOException ex)
        {
            return new NodeResult.Failure("IO_ERROR", ex.Message, fullPath);
        }
    }

    public IEnumerable<IFileSystemNode> GetChildren(int maxDepth = 1)
    {
        if (!Strategy.HasChildren || maxDepth <= 0)
            yield break;

        NodeResult childrenResult = _children.Value;

        if (childrenResult is NodeResult.Success success && success.Data is IEnumerable<IFileSystemNode> children)
        {
            if (maxDepth == 1)
            {
                foreach (IFileSystemNode child in children)
                {
                    yield return child;
                }

                yield break;
            }

            foreach (IFileSystemNode child in children)
            {
                yield return child;

                if (child.Strategy.HasChildren)
                {
                    foreach (IFileSystemNode grandChild in child.GetChildren(maxDepth - 1))
                    {
                        yield return grandChild;
                    }
                }
            }
        }
    }

    public void Accept(IFileSystemVisitor visitor)
    {
        visitor.Visit(this);
    }

    private NodeResult LoadChildren()
    {
        var children = new List<IFileSystemNode>();

        try
        {
            IEnumerable<IFileSystemNode> childrenFromStrategy = Strategy.GetChildren(this);

            foreach (IFileSystemNode child in childrenFromStrategy)
            {
                children.Add(child);
            }

            return new NodeResult.Success(children);
        }
        catch (UnauthorizedAccessException)
        {
            return new NodeResult.Failure("ACCESS_DENIED", "Access denied", Path);
        }
        catch (DirectoryNotFoundException)
        {
            return new NodeResult.Failure("NOT_FOUND", "Path not found", Path);
        }
        catch (IOException ex)
        {
            return new NodeResult.Failure("IO_ERROR", ex.Message, Path);
        }
    }
}