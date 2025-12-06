using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.CommandHandlers;

public class FileDeleteCommandHandler : BaseCommandHandler
{
    public override bool CanHandle(IReadOnlyList<string> parts)
    {
        return parts.Count >= 2 &&
               parts[0].Equals("file", StringComparison.OrdinalIgnoreCase) &&
               parts[1].Equals("delete", StringComparison.OrdinalIgnoreCase);
    }

    protected override ParsedCommand Process(IReadOnlyList<string> parts)
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
}