using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.CommandHandlers;

public class FileCopyCommandHandler : BaseCommandHandler
{
    public override bool CanHandle(IReadOnlyList<string> parts)
    {
        return parts.Count >= 2 &&
               parts[0].Equals("file", StringComparison.OrdinalIgnoreCase) &&
               parts[1].Equals("copy", StringComparison.OrdinalIgnoreCase);
    }

    protected override ParsedCommand Process(IReadOnlyList<string> parts)
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
}