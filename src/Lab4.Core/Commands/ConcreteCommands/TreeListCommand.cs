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
            return new FileSystemResult.Failure("Tree List", "Not connected");

        if (_context.CurrentPath == null)
            return new FileSystemResult.Failure("Tree List", "Current path is not set");

        try
        {
            IFileSystem fileSystem = _context.GetFileSystemOrThrow();
            FileSystemResult result = fileSystem.GetChildren(_context.CurrentPath, _depth);

            if (result is FileSystemResult.Success success)
            {
                return success;
            }

            return result;
        }
        catch (InvalidOperationException ex)
        {
            return new FileSystemResult.Failure("Tree List", ex.Message);
        }
    }
}