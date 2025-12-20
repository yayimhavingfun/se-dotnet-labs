using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.CommandHandlers;

public class FileMoveCommandHandler : BaseCommandHandler
{
    public override bool CanHandle(IReadOnlyList<string> parts)
    {
        return parts.Count >= 2 &&
               parts[0].Equals("file", StringComparison.OrdinalIgnoreCase) &&
               parts[1].Equals("move", StringComparison.OrdinalIgnoreCase);
    }

    protected override ParsedCommand Process(IReadOnlyList<string> parts)
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
}