using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures.Abilities;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Factories.CreatureFactories;

public class BattleAnalystFactory : CreatureFactory
{
    public BattleAnalystFactory() : base("Battle Analyst", 2, 4) { }

    public override (ICreature Creature, IModifierApplicator ModifierApplicator, ISpellApplicator SpellApplicator)
        Create()
    {
        var baseCreature = new Creature(Name, Attack, Health);
        var battleAnalyst = new BattleAnalyst(baseCreature);

        IModifierApplicator modifierApplicator = CreateModifierApplicator();
        ISpellApplicator spellApplicator = CreateSpellApplicator(modifierApplicator);

        return (battleAnalyst, modifierApplicator, spellApplicator);
    }
}