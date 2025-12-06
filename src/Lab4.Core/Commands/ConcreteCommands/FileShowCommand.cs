using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.ConcreteCommands;

public class FileShowCommand : ICommand
{
    private readonly ApplicationContext _context;
    private readonly string _path;
    private readonly string _mode;

    public FileShowCommand(ApplicationContext context, string path, string mode = "console")
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        _path = !string.IsNullOrWhiteSpace(path)
            ? path
            : throw new ArgumentException("Path cannot be empty", nameof(path));

        _mode = mode.ToLowerInvariant();
    }

    public FileSystemResult Execute()
    {
        if (!_context.IsConnected)
            return new FileSystemResult.Failure("File Show", "Not connected");

        if (_mode != "console")
            return new FileSystemResult.Failure("FileShow", $"Mode '{_mode}' is not supported");

        try
        {
            IFileSystem fileSystem = _context.GetFileSystemOrThrow();
            FileSystemResult result = fileSystem.ReadFile(_path);

            if (result is FileSystemResult.Success)
            {
                return new FileSystemResult.Success("File Show", "File showed successfully");
            }

            return result;
        }
        catch (InvalidOperationException ex)
        {
            return new FileSystemResult.Failure("File Show", ex.Message);
        }
    }
}