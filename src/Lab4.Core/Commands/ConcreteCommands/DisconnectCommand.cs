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
            return new FileSystemResult.Failure("Disconnect", "Not connected");

        try
        {
            IFileSystem fileSystem = _context.GetFileSystemOrThrow();
            FileSystemResult result = fileSystem.Disconnect();

            if (result is FileSystemResult.Success)
            {
                _context.Disconnect();
                return new FileSystemResult.Success("Disconnect", "Disconnected successfully");
            }

            return result;
        }
        catch (InvalidOperationException ex)
        {
            return new FileSystemResult.Failure("Disconnect", ex.Message);
        }
    }
}