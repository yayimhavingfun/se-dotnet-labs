using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.ConcreteModifiers;

public class DoubleStrikeModifier
{
    public string Name => "Double Strike";

    private bool IsActive { get; set; } = true;

    public void ModifyAttack(ICreature creature, ICreature target)
    {
        creature.Attack(target);
        if (target.CanBeTargeted && IsActive)
        {
            creature.Attack(target);
            IsActive = false;
        }
    }
}