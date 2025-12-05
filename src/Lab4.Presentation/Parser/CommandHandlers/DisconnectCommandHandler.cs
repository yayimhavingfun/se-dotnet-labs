using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.CommandHandlers;

public class DisconnectCommandHandler : BaseCommandHandler
{
    public override bool CanHandle(IReadOnlyList<string> parts)
    {
        return parts.Count == 1 &&
               parts[0].Equals("disconnect", StringComparison.OrdinalIgnoreCase);
    }

    protected override ParsedCommand Process(IReadOnlyList<string> parts)
    {
        return new ParsedCommand
        {
            CommandName = "disconnect",
            Parameters = new Dictionary<string, string>(),
            Arguments = new List<string>(),
            Flags = new Dictionary<string, string>(),
        };
    }
}