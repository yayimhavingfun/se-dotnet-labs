using Itmo.ObjectOrientedProgramming.Lab4.Core;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands.ConcreteCommands;
using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation;

public class CommandFactory
{
    private readonly ApplicationContext _context;

    public CommandFactory(ApplicationContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public ICommand Create(ParsedCommand parsedCommand)
    {
        ArgumentNullException.ThrowIfNull(parsedCommand);

        return parsedCommand.CommandName switch
        {
            "connect" => CreateConnectCommand(parsedCommand),
            "disconnect" => CreateDisconnectCommand(parsedCommand),
            "tree_goto" => CreateTreeGoToCommand(parsedCommand),
            "tree_list" => CreateTreeListCommand(parsedCommand),
            "file_show" => CreateFileShowCommand(parsedCommand),
            "file_move" => CreateFileMoveCommand(parsedCommand),
            "file_copy" => CreateFileCopyCommand(parsedCommand),
            "file_delete" => CreateFileDeleteCommand(parsedCommand),
            "file_rename" => CreateFileRenameCommand(parsedCommand),
            _ => throw new ArgumentException($"Unknown command: {parsedCommand.CommandName}"),
        };
    }

    private ConnectCommand CreateConnectCommand(ParsedCommand parsed)
    {
        string address = GetRequiredParameter(parsed, "address");
        string mode = GetOptionalParameter(parsed, "mode", "local");
        return new ConnectCommand(_context, address, mode);
    }

    private DisconnectCommand CreateDisconnectCommand(ParsedCommand parsed)
    {
        return new DisconnectCommand(_context);
    }

    private TreeGotoCommand CreateTreeGoToCommand(ParsedCommand parsed)
    {
        string path = GetRequiredParameter(parsed, "path");
        return new TreeGotoCommand(_context, path);
    }

    private TreeListCommand CreateTreeListCommand(ParsedCommand parsed)
    {
        string depthStr = GetRequiredParameter(parsed, "depth");

        if (!int.TryParse(depthStr, out int depth) || depth <= 0)
            throw new ArgumentException("Depth must be a positive integer");

        return new TreeListCommand(_context, depth);
    }

    private FileShowCommand CreateFileShowCommand(ParsedCommand parsed)
    {
        string path = GetRequiredParameter(parsed, "path");
        string mode = GetRequiredParameter(parsed, "mode");

        if (mode != "console")
            throw new ArgumentException($"Mode '{mode}' not supported. Use 'console'");

        return new FileShowCommand(_context, path, mode);
    }

    private FileMoveCommand CreateFileMoveCommand(ParsedCommand parsed)
    {
        string source = GetRequiredParameter(parsed, "source");
        string destination = GetRequiredParameter(parsed, "destination");
        return new FileMoveCommand(_context, source, destination);
    }

    private FileCopyCommand CreateFileCopyCommand(ParsedCommand parsed)
    {
        string source = GetRequiredParameter(parsed, "source");
        string destination = GetRequiredParameter(parsed, "destination");
        return new FileCopyCommand(_context, source, destination);
    }

    private FileDeleteCommand CreateFileDeleteCommand(ParsedCommand parsed)
    {
        string path = GetRequiredParameter(parsed, "path");
        return new FileDeleteCommand(_context, path);
    }

    private FileRenameCommand CreateFileRenameCommand(ParsedCommand parsed)
    {
        string path = GetRequiredParameter(parsed, "path");
        string newName = GetRequiredParameter(parsed, "newName");
        return new FileRenameCommand(_context, path, newName);
    }

    private string GetRequiredParameter(ParsedCommand parsed, string key)
    {
        if (!parsed.Parameters.TryGetValue(key, out string? value) || string.IsNullOrEmpty(value))
            throw new ArgumentException($"Required parameter '{key}' is missing");
        return value;
    }

    private string GetOptionalParameter(
        ParsedCommand parsed,
        string key,
        string defaultValue)
    {
        return parsed.Parameters.GetValueOrDefault(key, defaultValue);
    }
}
