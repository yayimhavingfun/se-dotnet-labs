using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.CommandHandlers;

public class TreeCommandHandler : BaseCommandHandler
{
    public override bool CanHandle(IReadOnlyList<string> parts)
    {
        return parts.Count >= 2 &&
               parts[0].Equals("tree", StringComparison.OrdinalIgnoreCase) &&
               (parts[1].Equals("goto", StringComparison.OrdinalIgnoreCase) ||
                parts[1].Equals("list", StringComparison.OrdinalIgnoreCase));
    }

    protected override ParsedCommand Process(IReadOnlyList<string> parts)
    {
        string subcommand = parts[1].ToLowerInvariant();

        return subcommand switch
        {
            "goto" => ProcessTreeGoto(parts),
            "list" => ProcessTreeList(parts),
            _ => throw new ArgumentOutOfRangeException(nameof(parts), "Unknown tree subcommand"),
        };
    }

    private ParsedCommand ProcessTreeGoto(IReadOnlyList<string> parts)
    {
        if (parts.Count < 3)
            throw new ArgumentException("Tree goto command requires path");

        var parameters = new Dictionary<string, string>();
        var arguments = new List<string>();

        string path = parts[2];
        arguments.Add(path);
        parameters["path"] = path;

        return new ParsedCommand
        {
            CommandName = "tree_goto",
            Parameters = parameters,
            Arguments = arguments,
            Flags = new Dictionary<string, string>(),
        };
    }

    private ParsedCommand ProcessTreeList(IReadOnlyList<string> parts)
    {
        Dictionary<string, string> flags = ParseFlags(parts, 2);

        if (!flags.TryGetValue("d", out string? value))
            throw new ArgumentException("Tree list command requires -d flag");

        var parameters = new Dictionary<string, string>
        {
            ["depth"] = value,
        };

        if (!int.TryParse(value, out int depth) || depth <= 0)
            throw new ArgumentException("Depth must be a positive integer");

        return new ParsedCommand
        {
            CommandName = "tree_list",
            Parameters = parameters,
            Arguments = new List<string>(),
            Flags = flags,
        };
    }
}