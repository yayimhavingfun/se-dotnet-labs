using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.ConcreteCommands;

public class FileRenameCommand : ICommand
{
    private readonly ApplicationContext _context;
    private readonly string _path;
    private readonly string _newName;

    public FileRenameCommand(ApplicationContext context, string path, string newName)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _path = path ?? throw new ArgumentNullException(nameof(path));
        _newName = newName ?? throw new ArgumentNullException(nameof(newName));
    }

    public FileSystemResult Execute()
    {
        if (!_context.IsConnected)
            return new FileSystemResult.NotConnected();

        if (_context.CurrentFileSystem == null)
            return new FileSystemResult.OperationFailed("FileRename", "File system is not available");

        if (string.IsNullOrWhiteSpace(_path))
            return new FileSystemResult.OperationFailed("FileRename", "Path is empty");

        if (string.IsNullOrWhiteSpace(_newName))
            return new FileSystemResult.OperationFailed("FileRename", "New name is empty");

        return _context.CurrentFileSystem.RenameFile(_path, _newName);
    }
}