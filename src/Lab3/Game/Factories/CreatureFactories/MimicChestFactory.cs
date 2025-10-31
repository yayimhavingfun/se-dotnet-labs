using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures.Abilities;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Factories.CreatureFactories;

public class MimicChestFactory : CreatureFactory
{
    public MimicChestFactory() : base("Mimic Chest",  1, 1) { }

    public override (ICreature Creature, IModifierApplicator ModifierApplicator, ISpellApplicator SpellApplicator)
        Create()
    {
        var baseCreature = new Creature(Name, Attack, Health);
        var mimicChest = new MimicChest(baseCreature);

        IModifierApplicator modifierApplicator = CreateModifierApplicator();
        ISpellApplicator spellApplicator = CreateSpellApplicator(modifierApplicator);

        return (mimicChest, modifierApplicator, spellApplicator);
    }
}