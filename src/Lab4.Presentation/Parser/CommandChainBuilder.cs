namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser;

public class CommandChainBuilder
{
    private ICommandHandler? _firstHandler = null;
    private ICommandHandler? _lastHandler = null;

    public CommandChainBuilder AddHandler(ICommandHandler handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        if (_firstHandler == null)
        {
            _firstHandler = handler;
            _lastHandler = handler;
        }
        else
        {
            if (_lastHandler != null)
            {
                _lastHandler.SetNext(handler);
            }

            _lastHandler = handler;
        }

        return this;
    }

    public ICommandHandler Build()
    {
        if (_firstHandler == null)
        {
            throw new InvalidOperationException(
                "Command chain must have at least one handler");
        }

        return _firstHandler;
    }
}