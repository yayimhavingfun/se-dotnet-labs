using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Factories;

public interface ICreatureFactory
{
    string CreatureType { get; }

    ICreature Create();
}