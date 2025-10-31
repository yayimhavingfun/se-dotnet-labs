using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures.Abilities;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Factories.CreatureFactories;

public class ViciousFighterFactory : CreatureFactory
{
    public ViciousFighterFactory() : base("Vicious Fighter", 1, 6) { }

    public override (ICreature Creature, IModifierApplicator ModifierApplicator, ISpellApplicator SpellApplicator)
        Create()
    {
        var baseCreature = new Creature(Name, Attack, Health);
        var viciousFighter = new ViciousFighter(baseCreature);

        IModifierApplicator modifierApplicator = CreateModifierApplicator();
        ISpellApplicator spellApplicator = CreateSpellApplicator(modifierApplicator);

        return (viciousFighter, modifierApplicator, spellApplicator);
    }
}