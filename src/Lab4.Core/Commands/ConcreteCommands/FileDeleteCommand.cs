using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.ConcreteCommands;

public class FileDeleteCommand : ICommand
{
    private readonly ApplicationContext _context;
    private readonly string _path;

    public FileDeleteCommand(ApplicationContext context, string path)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public FileSystemResult Execute()
    {
        if (!_context.IsConnected)
            return new FileSystemResult.NotConnected();

        if (_context.CurrentFileSystem == null)
            return new FileSystemResult.OperationFailed("FileDelete", "File system is not available");

        if (string.IsNullOrWhiteSpace(_path))
            return new FileSystemResult.OperationFailed("FileDelete", "Path is empty");

        return _context.CurrentFileSystem.DeleteFile(_path);
    }
}