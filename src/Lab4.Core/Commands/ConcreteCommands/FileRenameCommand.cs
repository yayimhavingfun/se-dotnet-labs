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

        _path = !string.IsNullOrWhiteSpace(path)
            ? path
            : throw new ArgumentException("Source path cannot be empty", nameof(path));

        _newName = string.IsNullOrWhiteSpace(newName)
            ? newName
            : throw new ArgumentException("Destination path cannot be empty", nameof(newName));
    }

    public FileSystemResult Execute()
    {
        if (!_context.IsConnected)
            return new FileSystemResult.Failure("File Rename", "Not connected");

        try
        {
            IFileSystem fileSystem = _context.GetFileSystemOrThrow();
            FileSystemResult result = fileSystem.MoveFile(_path, _newName);

            if (result is FileSystemResult.Success)
            {
                return new FileSystemResult.Success("File Rename", "File renamed successfully");
            }

            return result;
        }
        catch (InvalidOperationException ex)
        {
            return new FileSystemResult.Failure("File Rename", ex.Message);
        }
    }
}