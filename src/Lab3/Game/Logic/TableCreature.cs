using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Logic;

public class TableCreature
{
    public TableCreature(ICreature creature, IModifierApplicator modifierApplicator, ISpellApplicator spellApplicator)
    {
        Creature = creature;
        ModifierApplicator = modifierApplicator;
        SpellApplicator = spellApplicator;
    }

    public ICreature Creature { get; }

    public IModifierApplicator ModifierApplicator { get; }

    public ISpellApplicator SpellApplicator { get; }
}