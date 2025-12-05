using Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser.Models;

namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation.Parser;

public interface ICommandParser
{
    ParsedCommand Parse(string input);

    bool CanParse(string input);
}