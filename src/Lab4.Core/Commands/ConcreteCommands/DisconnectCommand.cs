using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.ConcreteCommands;

public class DisconnectCommand : ICommand
{
    private readonly ApplicationContext _context;

    public DisconnectCommand(ApplicationContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public FileSystemResult Execute()
    {
        if (!_context.IsConnected)
            return new FileSystemResult.NotConnected();

        if (_context.CurrentFileSystem == null)
            return new FileSystemResult.OperationFailed("Disconnect", "File system is not initialized");

        FileSystemResult result = _context.CurrentFileSystem.Disconnect();

        if (result is FileSystemResult.Disconnected)
        {
            _context.Disconnect();
        }

        return new FileSystemResult.Disconnected();
    }
}