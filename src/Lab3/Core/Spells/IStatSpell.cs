using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;

public interface IStatSpell : ISpell
{
    void ChangeStats(ICreature creature);
}