using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Factories.CreatureFactories;

public class ImmortalHorrorFactory : CreatureFactory
{
    public override string CreatureType => "Immortal Horror";

    public override ICreature Create()
    {
        return new ImmortalHorror();
    }
}