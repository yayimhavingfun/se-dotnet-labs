using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Factories;

public abstract class CreatureFactory : ICreatureFactory
{
    public abstract string CreatureType { get; }

    public abstract ICreature Create();
}