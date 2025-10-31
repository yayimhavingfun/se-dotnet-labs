using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Spells.Strategies;

public interface ISpellStrategy
{
    bool CanHandle(ISpell spell);

    void Handle(ISpell spell, ICreature creature);
}