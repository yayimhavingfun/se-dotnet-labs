using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.ConcreteCommands;

public class TreeGotoCommand : ICommand
{
    private readonly ApplicationContext _context;
    private readonly string _path;

    public TreeGotoCommand(ApplicationContext context, string path)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        _path = !string.IsNullOrWhiteSpace(path)
            ? path
            : throw new ArgumentException("Path cannot be empty", nameof(path));
    }

    public FileSystemResult Execute()
    {
        if (!_context.IsConnected)
            return new FileSystemResult.Failure("Tree Goto", "Not connected");

        try
        {
            IFileSystem fileSystem = _context.GetFileSystemOrThrow();
            FileSystemResult result = fileSystem.ChangeDirectory(_path);

            if (result is FileSystemResult.Success)
            {
                return new FileSystemResult.Success("Tree Goto", "Changed directories successfully");
            }

            return result;
        }
        catch (InvalidOperationException ex)
        {
            return new FileSystemResult.Failure("Tree Goto", ex.Message);
        }
    }
}