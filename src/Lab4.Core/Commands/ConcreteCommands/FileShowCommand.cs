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
        _path = path ?? throw new ArgumentNullException(nameof(path));
        _mode = mode.ToLowerInvariant();
    }

    public FileSystemResult Execute()
    {
        if (!_context.IsConnected)
            return new FileSystemResult.NotConnected();

        if (_context.CurrentFileSystem == null)
            return new FileSystemResult.OperationFailed("FileShow", "File system is not available");

        if (_mode != "console")
            return new FileSystemResult.OperationFailed("FileShow", $"Mode {_mode} is not supported");

        return _context.CurrentFileSystem.ReadFile(_path);
    }
}