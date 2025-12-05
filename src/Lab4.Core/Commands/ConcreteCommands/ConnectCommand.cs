using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem;
using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Implementation;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.ConcreteCommands;

public class ConnectCommand : ICommand
{
    private readonly ApplicationContext _context;
    private readonly string _address;
    private readonly string _mode;

    public ConnectCommand(ApplicationContext context, string address, string mode = "local")
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _address = address ?? throw new ArgumentNullException(nameof(address));
        _mode = mode.ToLowerInvariant() ?? "local";
    }

    public FileSystemResult Execute()
    {
        if (_context.IsConnected)
            return new FileSystemResult.AlreadyConnected();

        if (_mode != "local")
            return new FileSystemResult.OperationFailed("Connect", $"Mode '{_mode}' not supported");

        if (!IsAbsolutePath(_address))
        {
            return new FileSystemResult.OperationFailed(
                "Connect",
                $"Path '{_address}' is not absolute. Connect command requires absolute path.");
        }

        var fileSystem = new LocalFileSystem();

        FileSystemResult result = fileSystem.Connect(_address);

        if (result is FileSystemResult.Connected)
        {
            _context.Connect(fileSystem, _address);
        }

        return result;
    }

    private bool IsAbsolutePath(string path)
    {
        try
        {
            return Path.IsPathRooted(path) &&
                   !string.IsNullOrWhiteSpace(Path.GetPathRoot(path));
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}