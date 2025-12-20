namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;

public class ParsedCommand
{
    public required string CommandName { get; init; }

    public required Dictionary<string, string> Parameters { get; init; }

    public required IReadOnlyList<string> Arguments { get; init; }

    public required Dictionary<string, string> Flags { get; init; }
}