using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser;

public class CommandParser : ICommandParser
{
    private readonly ICommandHandler _handlerChain;
    private readonly CommandLineSplitter _splitter;

    public CommandParser(ICommandHandler handlerChain)
    {
        _splitter = new CommandLineSplitter();
        _handlerChain = handlerChain ?? throw new ArgumentNullException(nameof(handlerChain));
    }

    public CommandParser(Action<CommandChainBuilder> configure)
    {
        _splitter = new CommandLineSplitter();
        ArgumentNullException.ThrowIfNull(configure);

        var builder = new CommandChainBuilder();
        configure(builder);
        _handlerChain = builder.Build();
    }

    public ParsedCommand Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Input cannot be empty or whitespace");

        IReadOnlyList<string> parts = _splitter.Split(input);
        if (parts.Count == 0)
            throw new ArgumentException("No command provided");

        ParsedCommand? parsedCommand = _handlerChain.Handle(parts);

        if (parsedCommand == null)
            throw new ArgumentException($"Unknown command: {parts[0]}");

        return parsedCommand;
    }

    public bool CanParse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return false;

        try
        {
            IReadOnlyList<string> parts = _splitter.Split(input);
            if (parts.Count == 0)
                return false;

            return _handlerChain.Handle(parts) != null;
        }
        catch
        {
            return false;
        }
    }
}