using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Spells.ConcreteSpells;

public class StrengthPotion : IStatSpell
{
    public string Name => "Strength Potion";

    public void ChangeStats(ICreature creature)
    {
        creature.CurrentAttack += 5;
    }
}