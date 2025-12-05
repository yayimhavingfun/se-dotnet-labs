using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser;

public interface ICommandHandler
{
    ICommandHandler? SetNext(ICommandHandler handler);

    ParsedCommand? Handle(IReadOnlyList<string> parts);
}