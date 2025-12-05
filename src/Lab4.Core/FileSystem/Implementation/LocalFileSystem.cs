using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Nodes;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Implementation;

public class LocalFileSystem : IFileSystem
{
    private string? _rootPath;
    private string? _currentPath;

    public bool IsConnected => _rootPath != null;

    public FileSystemResult Connect(string address)
    {
        if (IsConnected)
            return new FileSystemResult.AlreadyConnected();

        string normalized = NormalizePath(address);
        if (!Directory.Exists(normalized))
            return new FileSystemResult.NotFound(address);

        _rootPath = normalized;
        _currentPath = normalized;
        return new FileSystemResult.Connected(address);
    }

    public FileSystemResult Disconnect()
    {
        if (!IsConnected)
            return new FileSystemResult.NotConnected();

        _rootPath = null;
        _currentPath = null;
        return new FileSystemResult.Disconnected();
    }

    public FileSystemResult ChangeDirectory(string path)
    {
        if (!IsConnected)
            return new FileSystemResult.NotConnected();

        string resolved = ResolvePath(path);
        if (!Directory.Exists(resolved))
            return new FileSystemResult.NotFound(path);

        if (!IsPathWithRoot(resolved))
            return new FileSystemResult.PathOutsideRoot(path);

        _currentPath = resolved;
        return new FileSystemResult.DirectoryChanged(resolved);
    }

    public FileSystemResult GetCurrentDirectory()
    {
        if (!IsConnected || _currentPath == null)
            return new FileSystemResult.NotConnected();

        return new FileSystemResult.CurrentDirectory(_currentPath);
    }

    public FileSystemResult GetNode(string path)
    {
        if (!IsConnected)
            return new FileSystemResult.NotConnected();

        string resolved = ResolvePath(path);

        NodeResult nodeResult = LocalFileSystemNode.TryCreate(resolved);

        return nodeResult switch
        {
            NodeResult.NodeCreated created =>
                new FileSystemResult.NodeInfo(created.Node),

            NodeResult.AccessDenied access =>
                new FileSystemResult.AccessDenied(access.Path),

            NodeResult.NotFound notFound =>
                new FileSystemResult.NotFound(notFound.Path),

            NodeResult.IoError io =>
                new FileSystemResult.OperationFailed("GetNode", io.Message),

            _ => new FileSystemResult.OperationFailed("GetNode", "Unknown error"),
        };
    }

    public FileSystemResult GetChildren(string path, int maxDepth = 1)
    {
        FileSystemResult nodeResult = GetNode(path);
        if (nodeResult is not FileSystemResult.NodeInfo nodeInfo)
            return nodeResult;

        if (!nodeInfo.Node.IsDirectory)
            return new FileSystemResult.NotADirectory(path);

        IEnumerable<IFileSystemNode> children = nodeInfo.Node.GetChildren(maxDepth);
        return new FileSystemResult.ChildrenList(children);
    }

    public FileSystemResult ReadFile(string path)
    {
        if (!IsConnected)
            return new FileSystemResult.NotConnected();

        string resolved = ResolvePath(path);

        NodeResult nodeResult = LocalFileSystemNode.TryCreate(resolved);
        if (nodeResult is not NodeResult.NodeCreated created)
            return HandleNodeError(nodeResult, "ReadFile");

        if (created.Node.IsDirectory)
            return new FileSystemResult.NotAFile(path);

        try
        {
            string content = File.ReadAllText(resolved);
            return new FileSystemResult.FileContent(content);
        }
        catch (UnauthorizedAccessException)
        {
            return new FileSystemResult.AccessDenied(path);
        }
        catch (IOException ex)
        {
            return new FileSystemResult.OperationFailed("ReadFile", ex.Message);
        }
    }

    public FileSystemResult MoveFile(string source, string destination)
    {
        if (!IsConnected)
            return new FileSystemResult.NotConnected();

        string src = ResolvePath(source);
        string dst = ResolvePath(destination);

        NodeResult sourceResult = LocalFileSystemNode.TryCreate(src);
        if (sourceResult is not NodeResult.NodeCreated sourceCreated)
            return HandleNodeError(sourceResult, "MoveFile");

        if (sourceCreated.Node.IsDirectory)
            return new FileSystemResult.OperationFailed("MoveFile", "Cannot move directories");

        if (Directory.Exists(dst))
            dst = Path.Combine(dst, Path.GetFileName(src));

        try
        {
            File.Move(src, dst);
            return new FileSystemResult.FileMoved();
        }
        catch (UnauthorizedAccessException)
        {
            return new FileSystemResult.AccessDenied(destination);
        }
        catch (IOException ex)
        {
            return new FileSystemResult.OperationFailed("MoveFile", ex.Message);
        }
    }

    public FileSystemResult CopyFile(string source, string destination)
    {
        if (!IsConnected)
            return new FileSystemResult.NotConnected();

        string src = ResolvePath(source);
        string dst = ResolvePath(destination);

        NodeResult sourceResult = LocalFileSystemNode.TryCreate(src);
        if (sourceResult is not NodeResult.NodeCreated sourceCreated)
            return HandleNodeError(sourceResult, "CopyFile");

        if (sourceCreated.Node.IsDirectory)
            return new FileSystemResult.OperationFailed("CopyFile", "Cannot copy directories");

        if (Directory.Exists(dst))
            dst = Path.Combine(dst, Path.GetFileName(src));

        try
        {
            File.Copy(src, dst, overwrite: false);
            return new FileSystemResult.FileCopied();
        }
        catch (UnauthorizedAccessException)
        {
            return new FileSystemResult.AccessDenied(destination);
        }
        catch (IOException ex) when (ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
        {
            return new FileSystemResult.OperationFailed("CopyFile", "File already exists");
        }
        catch (IOException ex)
        {
            return new FileSystemResult.OperationFailed("CopyFile", ex.Message);
        }
    }

    public FileSystemResult DeleteFile(string path)
    {
        if (!IsConnected)
            return new FileSystemResult.NotConnected();

        string resolved = ResolvePath(path);

        NodeResult nodeResult = LocalFileSystemNode.TryCreate(resolved);
        if (nodeResult is not NodeResult.NodeCreated created)
            return HandleNodeError(nodeResult, "DeleteFile");

        if (created.Node.IsDirectory)
            return new FileSystemResult.OperationFailed("DeleteFile", "Cannot delete directories");

        try
        {
            File.Delete(resolved);
            return new FileSystemResult.FileDeleted();
        }
        catch (UnauthorizedAccessException)
        {
            return new FileSystemResult.AccessDenied(path);
        }
        catch (IOException ex)
        {
            return new FileSystemResult.OperationFailed("DeleteFile", ex.Message);
        }
    }

    public FileSystemResult RenameFile(string path, string newName)
    {
        if (!IsConnected)
            return new FileSystemResult.NotConnected();

        string resolved = ResolvePath(path);

        NodeResult nodeResult = LocalFileSystemNode.TryCreate(resolved);
        if (nodeResult is not NodeResult.NodeCreated created)
            return HandleNodeError(nodeResult, "RenameFile");

        if (created.Node.IsDirectory)
            return new FileSystemResult.OperationFailed("RenameFile", "Cannot rename directories");

        if (newName.Contains(Path.DirectorySeparatorChar, StringComparison.Ordinal) ||
            newName.Contains(Path.AltDirectorySeparatorChar, StringComparison.Ordinal))
        {
            return new FileSystemResult.OperationFailed("RenameFile", "New name cannot contain path separators");
        }

        string? directory = Path.GetDirectoryName(resolved);
        if (directory == null)
            return new FileSystemResult.OperationFailed("RenameFile", "Invalid directory");

        string newPath = Path.Combine(directory, newName);

        if (File.Exists(newPath) || Directory.Exists(newPath))
            return new FileSystemResult.OperationFailed("RenameFile", "Name already exists");

        try
        {
            File.Move(resolved, newPath);
            return new FileSystemResult.FileRenamed();
        }
        catch (UnauthorizedAccessException)
        {
            return new FileSystemResult.AccessDenied(path);
        }
        catch (IOException ex)
        {
            return new FileSystemResult.OperationFailed("RenameFile", ex.Message);
        }
    }

    public FileSystemResult CheckExists(string path)
    {
        if (!IsConnected)
            return new FileSystemResult.NotConnected();

        string resolved = ResolvePath(path);
        return new FileSystemResult.PathExists(File.Exists(resolved) || Directory.Exists(resolved));
    }

    public string NormalizePath(string path)
    {
        return Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar);
    }

    public string CombinePath(string basePath, string relativePath)
    {
        return Path.Combine(basePath, relativePath);
    }

    public bool IsPathWithRoot(string path)
    {
        if (_rootPath == null)
            return false;

        return NormalizePath(path)
            .StartsWith(_rootPath + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsAbsolutePath(string path)
    {
        return Path.IsPathRooted(path);
    }

    public IFileSystem CreateNew()
    {
        return new LocalFileSystem();
    }

    private string ResolvePath(string path)
    {
        if (IsAbsolutePath(path))
        {
            string absolute = NormalizePath(path);
            if (!IsPathWithRoot(absolute))
                throw new InvalidOperationException($"Path {path} is outside of connected root");
            return absolute;
        }
        else
        {
            string? basePath = _currentPath ?? _rootPath;
            if (basePath == null)
                throw new InvalidOperationException("Not connected");

            return CombinePath(basePath, path);
        }
    }

    private FileSystemResult HandleNodeError(NodeResult result, string operation)
    {
        return result switch
        {
            NodeResult.AccessDenied access =>
                new FileSystemResult.AccessDenied(access.Path),

            NodeResult.NotFound notFound =>
                new FileSystemResult.NotFound(notFound.Path),

            NodeResult.IoError io =>
                new FileSystemResult.OperationFailed(operation, io.Message),

            _ => new FileSystemResult.OperationFailed(operation, "Unknown error"),
        };
    }
}