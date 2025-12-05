using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.ConcreteCommands;

public class TreeListCommand : ICommand
{
    private readonly ApplicationContext _context;
    private readonly int _depth;

    public TreeListCommand(ApplicationContext context, int depth = 1)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _depth = depth > 0 ? depth : 1;
    }

    public FileSystemResult Execute()
    {
        if (!_context.IsConnected)
            return new FileSystemResult.NotConnected();

        if (_context.CurrentPath == null)
            return new FileSystemResult.NotConnected();

        if (_context.CurrentFileSystem == null)
            return new FileSystemResult.OperationFailed("TreeList", "File system is not available");

        return _context.CurrentFileSystem.GetChildren(_context.CurrentPath, _depth);
    }
}