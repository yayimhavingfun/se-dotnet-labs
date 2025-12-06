using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.CommandHandlers;

public class TreeGotoCommandHandler : BaseCommandHandler
{
    public override bool CanHandle(IReadOnlyList<string> parts)
    {
        return parts.Count >= 2 &&
               parts[0].Equals("tree", StringComparison.OrdinalIgnoreCase) &&
               parts[1].Equals("goto", StringComparison.OrdinalIgnoreCase);
    }

    protected override ParsedCommand Process(IReadOnlyList<string> parts)
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
}