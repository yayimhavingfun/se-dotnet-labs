namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;

public interface IModifierSpell : ISpell
{
    object CreateModifier();
}