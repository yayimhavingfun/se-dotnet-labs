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
            return new FileSystemResult.Failure("Connect", "Already connected");

        string normalized = NormalizePath(address);
        if (!Directory.Exists(normalized))
            return new FileSystemResult.Failure("Connect", $"Directory not found: {address}");

        _rootPath = normalized;
        _currentPath = normalized;
        return new FileSystemResult.Success("Connect", $"Connect to directory: {address}");
    }

    public FileSystemResult Disconnect()
    {
        if (!IsConnected)
            return new FileSystemResult.Failure("Disconnect", "Not connected");

        _rootPath = null;
        _currentPath = null;
        return new FileSystemResult.Success("Disconnect", "Successfully disconnected");
    }

    public FileSystemResult ChangeDirectory(string path)
    {
        if (!IsConnected)
            return new FileSystemResult.Failure("ChangeDirectory", "Not connected");

        string resolved = ResolvePath(path);
        if (!Directory.Exists(resolved))
            return new FileSystemResult.Failure("ChangeDirectory", $"Path not found: {path}");

        if (!IsPathWithRoot(resolved))
            return new FileSystemResult.Failure("ChangeDirectory", $"Path is outside of connected root: {path}");

        _currentPath = resolved;
        return new FileSystemResult.Success("ChangeDirectory", $"Directory changed to: {resolved}");
    }

    public FileSystemResult GetCurrentDirectory()
    {
        if (!IsConnected || _currentPath == null)
            return new FileSystemResult.Failure("GetCurrentDirectory", "Not connected");

        return new FileSystemResult.Success("GetCurrentDirectory", "Current directory", _currentPath);
    }

    public FileSystemResult GetNode(string path)
    {
        if (!IsConnected)
            return new FileSystemResult.Failure("GetNode", "Not connected");

        string resolved = ResolvePath(path);
        NodeResult nodeResult = LocalFileSystemNode.TryCreate(resolved);

        if (nodeResult is NodeResult.Success success && success.Data is IFileSystemNode node)
        {
            return new FileSystemResult.Success("GetNode", string.Empty, node);
        }

        if (nodeResult is NodeResult.Failure failure)
        {
            return new FileSystemResult.Failure(
                "GetNode",
                $"{failure.ErrorCode}: {failure.Message}");
        }

        return new FileSystemResult.Failure("GetNode", "Unknown error");
    }

    public FileSystemResult GetChildren(string path, int maxDepth = 1)
    {
        FileSystemResult nodeResult = GetNode(path);
        if (nodeResult is FileSystemResult.Failure failure)
            return failure;

        if (nodeResult is FileSystemResult.Success success && success.Data is IFileSystemNode node)
        {
            if (node.Type != "Directory")
                return new FileSystemResult.Failure("GetChildren", $"Path is not a directory: {path}");

            IEnumerable<IFileSystemNode> children = node.GetChildren(maxDepth);
            return new FileSystemResult.Success("GetChildren", string.Empty, children);
        }

        return new FileSystemResult.Failure("GetChildren", "Node data is null");
    }

    public FileSystemResult ReadFile(string path)
    {
        if (!IsConnected)
            return new FileSystemResult.Failure("ReadFile", "Not connected");

        string resolved = ResolvePath(path);
        NodeResult nodeResult = LocalFileSystemNode.TryCreate(resolved);

        if (nodeResult is NodeResult.Failure failure)
            return new FileSystemResult.Failure("ReadFile", $"{failure.ErrorCode}: {failure.Message}");

        if (nodeResult is NodeResult.Success success && success.Data is IFileSystemNode node)
        {
            if (node.Type == "Directory")
                return new FileSystemResult.Failure("ReadFile", $"Path is not a file: {path}");

            try
            {
                string content = File.ReadAllText(resolved);
                return new FileSystemResult.Success("ReadFile", string.Empty, content);
            }
            catch (UnauthorizedAccessException)
            {
                return new FileSystemResult.Failure("ReadFile", $"Access denied: {path}");
            }
            catch (IOException ex)
            {
                return new FileSystemResult.Failure("ReadFile", $"IO error: {ex.Message}");
            }
        }

        return new FileSystemResult.Failure("ReadFile", "Node data is null");
    }

    public FileSystemResult MoveFile(string source, string destination)
    {
        if (!IsConnected)
            return new FileSystemResult.Failure("MoveFile", "Not connected");

        string src = ResolvePath(source);
        string dst = ResolvePath(destination);

        NodeResult sourceResult = LocalFileSystemNode.TryCreate(src);
        if (sourceResult is NodeResult.Failure failure)
            return new FileSystemResult.Failure("MoveFile", $"{failure.ErrorCode}: {failure.Message}");

        if (sourceResult is NodeResult.Success success && success.Data is IFileSystemNode node)
        {
            if (node.Type == "Directory")
                return new FileSystemResult.Failure("MoveFile", "Cannot move directories");
        }
        else
        {
            return new FileSystemResult.Failure("MoveFile", "Node data is null");
        }

        if (Directory.Exists(dst))
            dst = Path.Combine(dst, Path.GetFileName(src));

        try
        {
            File.Move(src, dst);
            return new FileSystemResult.Success("MoveFile", "File moved successfully");
        }
        catch (UnauthorizedAccessException)
        {
            return new FileSystemResult.Failure("MoveFile", $"Access denied: {destination}");
        }
        catch (IOException ex)
        {
            return new FileSystemResult.Failure("MoveFile", $"IO error: {ex.Message}");
        }
    }

    public FileSystemResult CopyFile(string source, string destination)
    {
        if (!IsConnected)
            return new FileSystemResult.Failure("CopyFile", "Not connected");

        string src = ResolvePath(source);
        string dst = ResolvePath(destination);

        NodeResult sourceResult = LocalFileSystemNode.TryCreate(src);
        if (sourceResult is NodeResult.Failure failure)
            return new FileSystemResult.Failure("CopyFile", $"{failure.ErrorCode}: {failure.Message}");

        if (sourceResult is NodeResult.Success success && success.Data is IFileSystemNode node)
        {
            if (node.Type == "Directory")
                return new FileSystemResult.Failure("CopyFile", "Cannot copy directories");
        }
        else
        {
            return new FileSystemResult.Failure("CopyFile", "Node data is null");
        }

        if (Directory.Exists(dst))
            dst = Path.Combine(dst, Path.GetFileName(src));

        try
        {
            File.Copy(src, dst, overwrite: false);
            return new FileSystemResult.Success("CopyFile", "File copied successfully");
        }
        catch (UnauthorizedAccessException)
        {
            return new FileSystemResult.Failure("CopyFile", $"Access denied: {destination}");
        }
        catch (IOException ex) when (ex.Message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
        {
            return new FileSystemResult.Failure("CopyFile", "File already exists at destination");
        }
        catch (IOException ex)
        {
            return new FileSystemResult.Failure("CopyFile", $"IO error: {ex.Message}");
        }
    }

    public FileSystemResult DeleteFile(string path)
    {
        if (!IsConnected)
            return new FileSystemResult.Failure("DeleteFile", "Not connected");

        string resolved = ResolvePath(path);
        NodeResult nodeResult = LocalFileSystemNode.TryCreate(resolved);

        if (nodeResult is NodeResult.Failure failure)
            return new FileSystemResult.Failure("DeleteFile", $"{failure.ErrorCode}: {failure.Message}");

        if (nodeResult is NodeResult.Success success && success.Data is IFileSystemNode node)
        {
            if (node.Type == "Directory")
                return new FileSystemResult.Failure("DeleteFile", "Cannot delete directories");

            try
            {
                File.Delete(resolved);
                return new FileSystemResult.Success("DeleteFile", "File deleted successfully");
            }
            catch (UnauthorizedAccessException)
            {
                return new FileSystemResult.Failure("DeleteFile", $"Access denied: {path}");
            }
            catch (IOException ex)
            {
                return new FileSystemResult.Failure("DeleteFile", $"IO error: {ex.Message}");
            }
        }

        return new FileSystemResult.Failure("DeleteFile", "Node data is null");
    }

    public FileSystemResult RenameFile(string path, string newName)
    {
        if (!IsConnected)
            return new FileSystemResult.Failure("RenameFile", "Not connected");

        string resolved = ResolvePath(path);
        NodeResult nodeResult = LocalFileSystemNode.TryCreate(resolved);

        if (nodeResult is NodeResult.Failure failure)
            return new FileSystemResult.Failure("RenameFile", $"{failure.ErrorCode}: {failure.Message}");

        if (nodeResult is NodeResult.Success success && success.Data is IFileSystemNode node)
        {
            if (node.Type == "Directory")
                return new FileSystemResult.Failure("RenameFile", "Cannot rename directories");

            if (newName.Contains(Path.DirectorySeparatorChar, StringComparison.Ordinal) ||
                newName.Contains(Path.AltDirectorySeparatorChar, StringComparison.Ordinal))
            {
                return new FileSystemResult.Failure("RenameFile", "New name cannot contain path separators");
            }

            string? directory = Path.GetDirectoryName(resolved);
            if (directory == null)
                return new FileSystemResult.Failure("RenameFile", "Invalid directory");

            string newPath = Path.Combine(directory, newName);

            if (File.Exists(newPath) || Directory.Exists(newPath))
                return new FileSystemResult.Failure("RenameFile", "Name already exists");

            try
            {
                File.Move(resolved, newPath);
                return new FileSystemResult.Success("RenameFile", "File renamed successfully");
            }
            catch (UnauthorizedAccessException)
            {
                return new FileSystemResult.Failure("RenameFile", $"Access denied: {path}");
            }
            catch (IOException ex)
            {
                return new FileSystemResult.Failure("RenameFile", $"IO error: {ex.Message}");
            }
        }

        return new FileSystemResult.Failure("RenameFile", "Node data is null");
    }

    public FileSystemResult CheckExists(string path)
    {
        if (!IsConnected)
            return new FileSystemResult.Failure("CheckExists", "Not connected");

        string resolved = ResolvePath(path);
        bool exists = File.Exists(resolved) || Directory.Exists(resolved);
        return new FileSystemResult.Success("CheckExists", string.Empty, exists);
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
}