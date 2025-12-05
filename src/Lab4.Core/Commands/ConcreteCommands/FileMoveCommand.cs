using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.ConcreteCommands;

public class FileMoveCommand : ICommand
{
    private readonly ApplicationContext _context;
    private readonly string _sourcePath;
    private readonly string _destinationPath;

    public FileMoveCommand(ApplicationContext context, string sourcePath, string destinationPath)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _sourcePath = sourcePath ?? throw new ArgumentNullException(nameof(sourcePath));
        _destinationPath = destinationPath ?? throw new ArgumentNullException(nameof(destinationPath));
    }

    public FileSystemResult Execute()
    {
        if (!_context.IsConnected)
            return new FileSystemResult.NotConnected();

        if (_context.CurrentFileSystem == null)
            return new FileSystemResult.OperationFailed("FileMove", "File system is not available");

        if (string.IsNullOrWhiteSpace(_sourcePath))
            return new FileSystemResult.OperationFailed("FileMove", "Source path is empty");

        if (string.IsNullOrWhiteSpace(_destinationPath))
            return new FileSystemResult.OperationFailed("FileMove", "Destination path is empty");

        return _context.CurrentFileSystem.MoveFile(_sourcePath, _destinationPath);
    }
}