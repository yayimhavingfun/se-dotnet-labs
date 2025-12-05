using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Factories.CreatureFactories;

public class ViciousFighterFactory : CreatureFactory
{
    public override string CreatureType => "Vicious Fighter";

    public override ICreature Create()
    {
        return new ViciousFighter();
    }
}