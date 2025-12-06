using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.CommandHandlers;

public class TreeListCommandHandler : BaseCommandHandler
{
    public override bool CanHandle(IReadOnlyList<string> parts)
    {
        return parts.Count >= 2 &&
               parts[0].Equals("tree", StringComparison.OrdinalIgnoreCase) &&
               parts[1].Equals("list", StringComparison.OrdinalIgnoreCase);
    }

    protected override ParsedCommand Process(IReadOnlyList<string> parts)
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