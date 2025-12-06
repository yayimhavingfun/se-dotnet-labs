using Itmo.ObjectOrientedProgramming.Lab4.Core;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem;
using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Nodes;
using Itmo.ObjectOrientedProgramming.Lab4.Core.FileSystem.Visitor;
using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser;
using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.CommandHandlers;
using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation;

public class ConsoleApplication
{
    private readonly ApplicationContext _context = new();
    private readonly CommandParser _parser;
    private readonly CommandCreator _commandCreator;
    private readonly TreePrintingVisitor _treeVisitor = new();
    private bool _isRunning = true;

    public ConsoleApplication()
    {
        _parser = CreateCommandParser();
        _commandCreator = new CommandCreator(_context);
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
        ICommand command = _commandCreator.Create(parsedCommand);
        FileSystemResult result = command.Execute();
        PrintResult(result);
    }

    private void PrintResult(FileSystemResult result)
    {
        if (result is FileSystemResult.Success success)
        {
            HandleSuccessResult(success);
        }
        else if (result is FileSystemResult.Failure failure)
        {
            Console.WriteLine($"Error: {failure.Operation} failed - {failure.Reason}");
        }
        else
        {
            Console.WriteLine($"Unknown result type: {result.GetType().Name}");
        }
    }

    private void HandleSuccessResult(FileSystemResult.Success success)
    {
        if (success.Data is IEnumerable<IFileSystemNode> children)
        {
            PrintTree(children);
        }
        else if (success.Data is string content)
        {
            Console.WriteLine($"=== File Content ===\n{content}\n==================");
        }
        else if (success.Data is bool exists)
        {
            Console.WriteLine($"Path exists: {exists}");
        }
        else if (success.Data is IFileSystemNode node)
        {
            Console.WriteLine($"Node info: {node.Name} ({node.Strategy.Type})");
        }
        else if (!string.IsNullOrEmpty(success.Comment))
        {
            Console.WriteLine($"Success: {success.Comment}");
        }
        else
        {
            Console.WriteLine($"{success.Operation} completed successfully");
        }
    }

    private void PrintTree(IEnumerable<IFileSystemNode> children, int depth = 1)
    {
        _treeVisitor.Clear();

        foreach (IFileSystemNode child in children)
        {
            child.Accept(_treeVisitor);
        }

        string treeOutput = _treeVisitor.GetResult();
        Console.WriteLine(treeOutput);
    }

    private CommandParser CreateCommandParser()
    {
        return new CommandParser(builder => builder
            .AddHandler(new ConnectCommandHandler())
            .AddHandler(new DisconnectCommandHandler())
            .AddHandler(new TreeGotoCommandHandler())
            .AddHandler(new TreeListCommandHandler())
            .AddHandler(new FileShowCommandHandler())
            .AddHandler(new FileMoveCommandHandler())
            .AddHandler(new FileCopyCommandHandler())
            .AddHandler(new FileDeleteCommandHandler())
            .AddHandler(new FileRenameCommandHandler()));
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