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

        _sourcePath = !string.IsNullOrWhiteSpace(sourcePath)
            ? sourcePath
            : throw new ArgumentException("Source path cannot be empty", nameof(sourcePath));

        _destinationPath = string.IsNullOrWhiteSpace(destinationPath)
            ? destinationPath
            : throw new ArgumentException("Destination path cannot be empty", nameof(destinationPath));
    }

    public FileSystemResult Execute()
    {
        if (!_context.IsConnected)
            return new FileSystemResult.Failure("File Move", "Not connected");

        try
        {
            IFileSystem fileSystem = _context.GetFileSystemOrThrow();
            FileSystemResult result = fileSystem.MoveFile(_sourcePath, _destinationPath);

            if (result is FileSystemResult.Success)
            {
                return new FileSystemResult.Success("File Move", "File moved successfully");
            }

            return result;
        }
        catch (InvalidOperationException ex)
        {
            return new FileSystemResult.Failure("File Move", ex.Message);
        }
    }
}