using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures.Abilities;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Factories.CreatureFactories;

public class ImmortalHorrorFactory : CreatureFactory
{
    public ImmortalHorrorFactory() : base("Immortal Horror", 4, 4) { }

    public override (ICreature Creature, IModifierApplicator ModifierApplicator, ISpellApplicator SpellApplicator)
        Create()
    {
        var baseCreature = new Creature(Name, Attack, Health);
        var immortalHorror = new ImmortalHorror(baseCreature);

        IModifierApplicator modifierApplicator = CreateModifierApplicator();
        ISpellApplicator spellApplicator = CreateSpellApplicator(modifierApplicator);

        return (immortalHorror, modifierApplicator, spellApplicator);
    }
}