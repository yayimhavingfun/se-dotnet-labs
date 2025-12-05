using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Spells.ConcreteSpells;

public class StaminaPotion : IStatSpell
{
    public string Name => "Stamina Potion";

    public void ChangeStats(ICreature creature)
    {
        creature.CurrentHealth += 5;
    }
}