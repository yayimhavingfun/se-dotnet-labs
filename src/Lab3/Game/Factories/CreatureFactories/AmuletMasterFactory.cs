using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.ConcreteModifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.Strategies;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Factories.CreatureFactories;

public class AmuletMasterFactory : CreatureFactory
{
    public AmuletMasterFactory() : base("Amulet Master", 5, 2) { }

    protected override IModifierApplicator CreateModifierApplicator()
    {
        var applicator = new ModifierApplicator(new AttackStrategy(), new DefenseStrategy());

        applicator.AddModifier(new MagicShieldModifier());
        applicator.AddModifier(new DoubleStrikeModifier());

        return applicator;
    }
}