using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.ConcreteModifiers;

public class MagicShieldModifier
{
    public string Name => "Magic Shield";

    private bool IsActive { get; set; } = true;

    public void ModifyTakeDamage(ICreature creature, int damage)
    {
        if (creature.CanBeTargeted && IsActive)
        {
            IsActive = false;
            return;
        }

        creature.TakeDamage(damage);
    }
}