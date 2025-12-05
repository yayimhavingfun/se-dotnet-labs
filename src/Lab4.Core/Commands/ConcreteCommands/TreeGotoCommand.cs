using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.ConcreteCommands;

public class TreeGotoCommand : ICommand
{
    private readonly ApplicationContext _context;
    private readonly string _path;

    public TreeGotoCommand(ApplicationContext context, string path)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public FileSystemResult Execute()
    {
        if (!_context.IsConnected)
            return new FileSystemResult.NotConnected();

        if (_context.CurrentFileSystem == null)
            return new FileSystemResult.OperationFailed("Disconnect", "File system is not initialized");

        FileSystemResult result = _context.CurrentFileSystem.ChangeDirectory(_path);

        if (result is FileSystemResult.DirectoryChanged changed)
        {
            _context.CurrentPath = changed.NewPath;
        }

        return result;
    }
}