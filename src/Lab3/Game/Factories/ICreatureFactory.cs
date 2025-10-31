using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Factories;

public interface ICreatureFactory
{
    string Name { get; }

    (ICreature Creature, IModifierApplicator ModifierApplicator, ISpellApplicator SpellApplicator) Create();
}