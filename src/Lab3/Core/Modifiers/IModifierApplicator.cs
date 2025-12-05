using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;

public interface IModifierApplicator
{
    ICreature ActivateModifier(ICreature creature, Type modifierType);
}