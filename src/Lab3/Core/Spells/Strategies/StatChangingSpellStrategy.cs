using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Spells.Strategies;

public class StatChangingSpellStrategy : ISpellStrategy
{
    public bool CanHandle(ISpell spell)
    {
        return spell is IStatSpell;
    }

    public void Handle(ISpell spell, ICreature creature)
    {
        if (spell is IStatSpell statSpell)
            statSpell.ChangeStats(creature);
    }
}