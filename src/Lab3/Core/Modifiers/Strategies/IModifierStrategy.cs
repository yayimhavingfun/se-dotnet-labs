namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.Strategies;

public interface IModifierStrategy
{
    bool CanHandle(object modifier);
}