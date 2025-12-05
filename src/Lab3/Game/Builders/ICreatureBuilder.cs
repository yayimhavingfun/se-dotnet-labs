using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Builders;

public interface ICreatureBuilder
{
    ICreatureBuilder WithModifier(object modifier);

    ICreatureBuilder WithSpell(ISpell spell);

    ICreature Build();
}