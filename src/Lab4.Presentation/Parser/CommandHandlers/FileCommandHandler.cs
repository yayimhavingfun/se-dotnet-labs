using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.CommandHandlers;

public class FileCommandHandler : BaseCommandHandler
{
    public override bool CanHandle(IReadOnlyList<string> parts)
    {
        return parts.Count >= 2 && parts[0].Equals("file", StringComparison.OrdinalIgnoreCase);
    }

    protected override ParsedCommand Process(IReadOnlyList<string> parts)
    {
        string subcommand = parts[1].ToLowerInvariant();

        return subcommand switch
        {
            "show" => ProcessFileShow(parts),
            "move" => ProcessFileMove(parts),
            "copy" => ProcessFileCopy(parts),
            "delete" => ProcessFileDelete(parts),
            "rename" => ProcessFileRename(parts),
            _ => throw new ArgumentException($"Unknown file subcommand: {subcommand}"),
        };
    }

    private ParsedCommand ProcessFileShow(IReadOnlyList<string> parts)
    {
        IReadOnlyList<string> arguments = GetArguments(parts, 2, 1);
        if (arguments.Count == 0)
            throw new ArgumentException("File show command requires path");

        Dictionary<string, string> flags = ParseFlags(parts, 2);

        if (!flags.TryGetValue("m", out string? flag))
            throw new ArgumentException("File show command requires -m flag");

        var parameters = new Dictionary<string, string>
        {
            ["path"] = arguments[0],
            ["mode"] = flag,
        };

        return new ParsedCommand
        {
            CommandName = "file_show",
            Parameters = parameters,
            Arguments = arguments,
            Flags = flags,
        };
    }

    private ParsedCommand ProcessFileMove(IReadOnlyList<string> parts)
    {
        IReadOnlyList<string> arguments = GetArguments(parts, 2, 2);
        if (arguments.Count < 2)
            throw new ArgumentException("File move command requires source and destination paths");

        Dictionary<string, string> flags = ParseFlags(parts, 2);
        if (flags.Count > 0)
            throw new ArgumentException("File move command does not support flags");

        var parameters = new Dictionary<string, string>
        {
            ["source"] = arguments[0],
            ["destination"] = arguments[1],
        };
        return new ParsedCommand
        {
            CommandName = "file_move",
            Parameters = parameters,
            Arguments = arguments,
            Flags = new Dictionary<string, string>(),
        };
    }

    private ParsedCommand ProcessFileCopy(IReadOnlyList<string> parts)
    {
        IReadOnlyList<string> arguments = GetArguments(parts, 2, 2);
        if (arguments.Count < 2)
            throw new ArgumentException("File copy command requires source and destination paths");

        Dictionary<string, string> flags = ParseFlags(parts, 2);
        if (flags.Count > 0)
            throw new ArgumentException("File copy command does not support flags");

        var parameters = new Dictionary<string, string>
        {
            ["source"] = arguments[0],
            ["destination"] = arguments[1],
        };
        return new ParsedCommand
        {
            CommandName = "file_copy",
            Parameters = parameters,
            Arguments = arguments,
            Flags = new Dictionary<string, string>(),
        };
    }

    private ParsedCommand ProcessFileDelete(IReadOnlyList<string> parts)
    {
        IReadOnlyList<string> arguments = GetArguments(parts, 2, 1);
        if (arguments.Count == 0)
            throw new ArgumentException("File delete command requires path");

        Dictionary<string, string> flags = ParseFlags(parts, 2);
        if (flags.Count > 0)
            throw new ArgumentException("File delete command does not support flags");

        var parameters = new Dictionary<string, string>
        {
            ["path"] = arguments[0],
        };
        return new ParsedCommand
        {
            CommandName = "file_delete",
            Parameters = parameters,
            Arguments = arguments,
            Flags = new Dictionary<string, string>(),
        };
    }

    private ParsedCommand ProcessFileRename(IReadOnlyList<string> parts)
    {
        IReadOnlyList<string> arguments = GetArguments(parts, 2, 2);
        if (arguments.Count < 2)
            throw new ArgumentException("File rename command requires path and new name");

        Dictionary<string, string> flags = ParseFlags(parts, 2);
        if (flags.Count > 0)
            throw new ArgumentException("File rename command does not support flags");

        var parameters = new Dictionary<string, string>
        {
            ["path"] = arguments[0],
            ["newName"] = arguments[1],
        };
        return new ParsedCommand
        {
            CommandName = "file_rename",
            Parameters = parameters,
            Arguments = arguments,
            Flags = new Dictionary<string, string>(),
        };
    }
}