using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.ConcreteModifiers;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Factories.CreatureFactories;

public class AmuletMasterFactory : ICreatureFactory
{
    public string CreatureType => "Amulet Master";

    public ICreature Create()
    {
        var baseCreature = new BasicCreature("Amulet Master", 5, 2);

        var withShield = new MagicShieldModifier(baseCreature);

        var withDoubleStrike = new DoubleStrikeModifier(withShield);

        return withDoubleStrike;
    }
}