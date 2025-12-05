using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core;

public class ApplicationContext
{
    public IFileSystem? CurrentFileSystem { get; set; }

    public string? CurrentPath { get; set; }

    public string? RootPath { get; set; }

    public bool IsConnected => CurrentFileSystem?.IsConnected ?? false;

    public void Connect(IFileSystem fileSystem, string address)
    {
        CurrentFileSystem = fileSystem;
        RootPath = address;
        CurrentPath = address;
    }

    public void Disconnect()
    {
        CurrentFileSystem = null;
        RootPath = null;
        CurrentPath = null;
    }

    public void ChangeCurrentPath(string newPath)
    {
        CurrentPath = newPath;
    }
}