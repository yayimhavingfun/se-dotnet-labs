using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.CommandHandlers;

public class ConnectCommandHandler : BaseCommandHandler
{
    public override bool CanHandle(IReadOnlyList<string> parts)
    {
        return parts.Count >= 2 && parts[0].Equals("connect", StringComparison.OrdinalIgnoreCase);
    }

    protected override ParsedCommand Process(IReadOnlyList<string> parts)
    {
        if (parts.Count < 2)
            throw new ArgumentException("Connect command requires address");

        IReadOnlyList<string> arguments = GetArgumentsSkippingFlags(parts, 1, 1);

        if (arguments.Count == 0)
            throw new ArgumentException("Connect command requires address");

        Dictionary<string, string> flags = ParseFlags(parts, 2, "m");

        var parameters = new Dictionary<string, string>
        {
            ["address"] = arguments[0],
        };

        if (flags.TryGetValue("m", out string? mode))
        {
            parameters["mode"] = mode;
        }
        else
        {
            parameters["mode"] = "local";
        }

        return new ParsedCommand
        {
            CommandName = "connect",
            Parameters = parameters,
            Arguments = arguments,
            Flags = flags,
        };
    }
}