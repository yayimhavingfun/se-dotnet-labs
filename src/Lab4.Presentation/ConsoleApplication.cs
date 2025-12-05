using Itmo.ObjectOrientedProgramming.Lab4.Core;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem;
using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Nodes;
using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser;
using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.CommandHandlers;
using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;
using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Visitor;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation;

public class ConsoleApplication
{
    private readonly ApplicationContext _context = new();
    private readonly CommandParser _parser;
    private readonly CommandFactory _commandFactory;
    private readonly TreePrintingVisitor _treeVisitor = new();
    private bool _isRunning = true;

    public ConsoleApplication()
    {
        _parser = CreateCommandParser();
        _commandFactory = new CommandFactory(_context);
    }

    public void Run()
    {
        Console.WriteLine("=== File System Manager ===");
        Console.WriteLine("Type 'help' for commands, 'exit' to quit\n");

        while (_isRunning)
        {
            try
            {
                Console.Write($"{_context.CurrentPath ?? "not connected"}> ");
                string? input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                    continue;

                ProcessInput(input);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        Console.WriteLine("Goodbye!");
    }

    private void ProcessInput(string input)
    {
        switch (input.ToLowerInvariant())
        {
            case "exit":
                _isRunning = false;
                break;
            case "help":
                ShowHelp();
                break;
            case "clear":
                Console.Clear();
                break;
            default:
                ExecuteCommand(input);
                break;
        }
    }

    private void ExecuteCommand(string input)
    {
        ParsedCommand parsedCommand = _parser.Parse(input);
        ICommand command = _commandFactory.Create(parsedCommand);
        FileSystemResult result = command.Execute();
        PrintResult(result);
    }

    private void PrintResult(FileSystemResult result)
    {
        switch (result)
        {
            case FileSystemResult.ChildrenList childrenList:
                PrintTree(childrenList);
                break;

            case FileSystemResult.FileContent fileContent:
                Console.WriteLine($"=== File Content ===\n{fileContent.Content}\n==================");
                break;

            case FileSystemResult.Connected connected:
                Console.WriteLine($"Connected to: {connected.Address}");
                break;

            case FileSystemResult.Disconnected:
                Console.WriteLine("Disconnected successfully");
                break;

            case FileSystemResult.DirectoryChanged changed:
                Console.WriteLine($"Directory changed to: {changed.NewPath}");
                break;

            case FileSystemResult.FileMoved:
                Console.WriteLine("File moved successfully");
                break;

            case FileSystemResult.FileCopied:
                Console.WriteLine("File copied successfully");
                break;

            case FileSystemResult.FileDeleted:
                Console.WriteLine("File deleted successfully");
                break;

            case FileSystemResult.FileRenamed:
                Console.WriteLine("File renamed successfully");
                break;

            case FileSystemResult.NotConnected:
                Console.WriteLine("Error: Not connected to any file system");
                break;

            case FileSystemResult.AlreadyConnected:
                Console.WriteLine("Error: Already connected");
                break;

            case FileSystemResult.OperationFailed failed:
                Console.WriteLine($"Error: {failed.Operation} failed - {failed.Reason}");
                break;

            case FileSystemResult.NotFound notFound:
                Console.WriteLine($"Error: Path not found: {notFound.Path}");
                break;

            case FileSystemResult.AccessDenied accessDenied:
                Console.WriteLine($"Error: Access denied: {accessDenied.Path}");
                break;

            case FileSystemResult.NotADirectory notADir:
                Console.WriteLine($"Error: Not a directory: {notADir.Path}");
                break;

            case FileSystemResult.NotAFile notAFile:
                Console.WriteLine($"Error: Not a file: {notAFile.Path}");
                break;

            case FileSystemResult.PathOutsideRoot outside:
                Console.WriteLine($"Error: Path outside root: {outside.Path}");
                break;

            default:
                Console.WriteLine($"Unknown result type: {result.GetType().Name}");
                break;
        }
    }

    private void PrintTree(FileSystemResult.ChildrenList childrenList)
    {
        _treeVisitor.Clear();

        foreach (IFileSystemNode child in childrenList.Children)
        {
            if (child.IsDirectory)
            {
                IEnumerable<IFileSystemNode> grandChildren = child.GetChildren(1);
                _treeVisitor.VisitDirectory(child, grandChildren);
            }
            else
            {
                _treeVisitor.VisitFile(child);
            }
        }

        string treeOutput = _treeVisitor.GetResult();
        Console.WriteLine(treeOutput);
    }

    private CommandParser CreateCommandParser()
    {
        return new CommandParser(builder => builder
                .AddHandler(new ConnectCommandHandler())
                .AddHandler(new DisconnectCommandHandler())
                .AddHandler(new TreeCommandHandler())
                .AddHandler(new FileCommandHandler()));
    }

    private void ShowHelp()
    {
        Console.WriteLine("\nAvailable commands:");
        Console.WriteLine("  connect [address] [-m mode]      - Connect to file system");
        Console.WriteLine("  disconnect                       - Disconnect from file system");
        Console.WriteLine("  tree goto [path]                 - Change directory");
        Console.WriteLine("  tree list [-d depth]             - List directory tree");
        Console.WriteLine("  file show [path] [-m mode]       - Show file content");
        Console.WriteLine("  file move [source] [destination] - Move file");
        Console.WriteLine("  file copy [source] [destination] - Copy file");
        Console.WriteLine("  file delete [path]               - Delete file");
        Console.WriteLine("  file rename [path] [name]        - Rename file");
        Console.WriteLine("  clear                            - Clear console");
        Console.WriteLine("  help                             - Show this help");
        Console.WriteLine("  exit                             - Exit program\n");
    }
}