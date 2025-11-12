using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Factories.CreatureFactories;

public class MimicChestFactory : CreatureFactory
{
    public override string CreatureType => "Mimic Chest";

    public override ICreature Create()
    {
        return new MimicChest();
    }
}