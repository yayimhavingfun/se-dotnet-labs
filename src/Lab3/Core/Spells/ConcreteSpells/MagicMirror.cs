using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Spells.ConcreteSpells;

public class MagicMirror : IStatSpell
{
    public string Name => "Magic Mirror";

    public void ChangeStats(ICreature creature)
    {
        (creature.CurrentAttack, creature.CurrentHealth) = (creature.CurrentHealth, creature.CurrentAttack);
    }
}