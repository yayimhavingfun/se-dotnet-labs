namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Nodes;

public class LocalFileSystemNode : IFileSystemNode
{
    private readonly Lazy<NodeResult> _children;
    private readonly Lazy<FileSystemInfo> _info;

    public string Path { get; }

    public string Name => _info.Value.Name;

    public bool IsDirectory => _info.Value is DirectoryInfo;

    public long Size => IsDirectory ? 0 : ((FileInfo)_info.Value).Length;

    public DateTime LastModified => _info.Value.LastWriteTime;

    public LocalFileSystemNode(string fullPath)
    {
        Path = fullPath;

        _info = new Lazy<FileSystemInfo>(() =>
        {
            if (Directory.Exists(fullPath))
                return new DirectoryInfo(fullPath);

            return new FileInfo(fullPath);
        });

        _children = new Lazy<NodeResult>(() =>
        {
            FileSystemInfo info = _info.Value;
            if (info is DirectoryInfo)
                return LoadChildren();
            return new NodeResult.ChildrenLoaded([]);
        });
    }

    public static NodeResult TryCreate(string fullPath)
    {
        try
        {
            var node = new LocalFileSystemNode(fullPath);
            return new NodeResult.NodeCreated(node);
        }
        catch (UnauthorizedAccessException)
        {
            return new NodeResult.AccessDenied(fullPath);
        }
        catch (FileNotFoundException)
        {
            return new NodeResult.NotFound(fullPath);
        }
        catch (IOException ex)
        {
            return new NodeResult.IoError(fullPath, ex.Message);
        }
    }

    public IEnumerable<IFileSystemNode> GetChildren(int maxDepth = 1)
    {
        if (!IsDirectory || maxDepth <= 0)
            return [];

        NodeResult childrenResult = _children.Value;

        if (childrenResult is not NodeResult.ChildrenLoaded loaded)
            return [];

        if (maxDepth == 1)
            return loaded.Children;

        var allChildren = new List<IFileSystemNode>();
        foreach (IFileSystemNode child in loaded.Children)
        {
            allChildren.Add(child);

            if (child.IsDirectory)
            {
                allChildren.AddRange(child.GetChildren(maxDepth - 1));
            }
        }

        return allChildren;
    }

    private NodeResult LoadChildren()
    {
        var children = new List<IFileSystemNode>();
        var errors = new List<string>();

        try
        {
            foreach (string dir in Directory.EnumerateDirectories(Path))
            {
                NodeResult result = TryCreate(dir);
                if (result is NodeResult.NodeCreated created)
                    children.Add(created.Node);
                else
                    errors.Add($"{dir}: {result.GetType().Name}");
            }

            foreach (string file in Directory.EnumerateFiles(Path))
            {
                NodeResult result = TryCreate(file);
                if (result is NodeResult.NodeCreated created)
                    children.Add(created.Node);
                else
                    errors.Add($"{file}: {result.GetType().Name}");
            }

            if (errors.Count > 0)
            {
                Console.WriteLine($"Some items skipped: {string.Join(", ", errors)}");
            }

            return new NodeResult.ChildrenLoaded(children);
        }
        catch (UnauthorizedAccessException)
        {
            return new NodeResult.AccessDenied(Path);
        }
        catch (DirectoryNotFoundException)
        {
            return new NodeResult.NotFound(Path);
        }
        catch (IOException ex)
        {
            return new NodeResult.IoError(Path, ex.Message);
        }
    }
}