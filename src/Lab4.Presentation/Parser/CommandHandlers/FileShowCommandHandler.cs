using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.CommandHandlers;

public class FileShowCommandHandler : BaseCommandHandler
{
    public override bool CanHandle(IReadOnlyList<string> parts)
    {
        return parts.Count >= 2 &&
               parts[0].Equals("file", StringComparison.OrdinalIgnoreCase) &&
               parts[1].Equals("show", StringComparison.OrdinalIgnoreCase);
    }

    protected override ParsedCommand Process(IReadOnlyList<string> parts)
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
}