using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.ConcreteCommands;

public class FileDeleteCommand : ICommand
{
    private readonly ApplicationContext _context;
    private readonly string _path;

    public FileDeleteCommand(ApplicationContext context, string path)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        _path = !string.IsNullOrWhiteSpace(path)
            ? path
            : throw new ArgumentException("Source path cannot be empty", nameof(path));
    }

    public FileSystemResult Execute()
    {
        if (!_context.IsConnected)
            return new FileSystemResult.Failure("File Delete", "Not connected");

        try
        {
            IFileSystem fileSystem = _context.GetFileSystemOrThrow();
            FileSystemResult result = fileSystem.DeleteFile(_path);

            if (result is FileSystemResult.Success)
            {
                return new FileSystemResult.Success("File Delete", "File deleted successfully");
            }

            return result;
        }
        catch (InvalidOperationException ex)
        {
            return new FileSystemResult.Failure("File Delete", ex.Message);
        }
    }
}