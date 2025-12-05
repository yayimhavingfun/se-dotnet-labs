using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser;

public abstract class BaseCommandHandler :
    ICommandHandler
{
    private ICommandHandler? _nextHandler;

    public ICommandHandler SetNext(ICommandHandler handler)
    {
        _nextHandler = handler ?? throw new ArgumentNullException(nameof(handler));
        return handler;
    }

    public ParsedCommand? Handle(IReadOnlyList<string> parts)
    {
        if (CanHandle(parts))
        {
            return Process(parts);
        }

        return _nextHandler?.Handle(parts);
    }

    public abstract bool CanHandle(IReadOnlyList<string> parts);

    protected abstract ParsedCommand Process(IReadOnlyList<string> parts);

    protected Dictionary<string, string> ParseFlags(
        IReadOnlyList<string> parts,
        int startIndex,
        params string[] allowedFlags)
    {
        var flags = new Dictionary<string, string>();

        for (int i = startIndex; i < parts.Count; i++)
        {
            if (parts[i].StartsWith('-'))
            {
                string flagName = parts[i].TrimStart('-');

                if (allowedFlags.Length > 0 && !allowedFlags.Contains(flagName))
                {
                    throw new ArgumentException($"Unknown flag: {flagName}");
                }

                string? flagValue = null;
                if (i + 1 < parts.Count && !parts[i + 1].StartsWith('-'))
                {
                    flagValue = parts[i + 1];
                    i++;
                }

                flags[flagName] = flagValue ?? string.Empty;
            }
        }

        return flags;
    }

    protected IReadOnlyList<string> GetArguments(IReadOnlyList<string> parts, int startIndex, int expectedCount)
    {
        var arguments = new List<string>();
        int argIndex = 0;

        for (int i = startIndex; i < parts.Count && argIndex < expectedCount; i++)
        {
            if (!parts[i].StartsWith('-'))
            {
                arguments.Add(parts[i]);
                argIndex++;
            }
        }

        return arguments;
    }

    protected IReadOnlyList<string> GetArgumentsSkippingFlags(
        IReadOnlyList<string> parts,
        int startIndex,
        int expectedCount)
    {
        var arguments = new List<string>();
        int argIndex = 0;
        bool skipNext = false;

        for (int i = startIndex; i < parts.Count && argIndex < expectedCount; i++)
        {
            if (skipNext)
            {
                skipNext = false;
                continue;
            }

            if (parts[i].StartsWith('-'))
            {
                if (i + 1 < parts.Count && !parts[i + 1].StartsWith('-'))
                {
                    skipNext = true;
                }

                continue;
            }

            arguments.Add(parts[i]);
            argIndex++;
        }

        return arguments;
    }
}