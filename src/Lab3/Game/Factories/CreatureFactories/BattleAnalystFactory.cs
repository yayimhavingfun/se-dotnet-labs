using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Factories.CreatureFactories;

public class BattleAnalystFactory : CreatureFactory
{
    public override string CreatureType => "Battle Analyst";

    public override ICreature Create()
    {
        return new BattleAnalyst();
    }
}