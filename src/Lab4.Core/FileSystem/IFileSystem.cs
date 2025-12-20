namespace Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem;

public interface IFileSystem
{
    bool IsConnected { get; }

    // basic operations
    FileSystemResult Connect(string address);

    FileSystemResult Disconnect();

    FileSystemResult ChangeDirectory(string path);

    FileSystemResult GetCurrentDirectory();

    // node operations
    FileSystemResult GetNode(string path);

    FileSystemResult GetChildren(string path, int maxDepth = 1);

    // file operations
    FileSystemResult ReadFile(string path);

    FileSystemResult MoveFile(string source, string destination);

    FileSystemResult CopyFile(string source, string destination);

    FileSystemResult DeleteFile(string path);

    FileSystemResult RenameFile(string path, string newName);

    // path helpers
    FileSystemResult CheckExists(string path);

    string NormalizePath(string path);

    string CombinePath(string basePath, string relativePath);

    bool IsPathWithRoot(string path);

    bool IsAbsolutePath(string path);

    IFileSystem CreateNew();
}