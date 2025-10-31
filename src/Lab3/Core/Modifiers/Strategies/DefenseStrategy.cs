using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.Strategies;

public class DefenseStrategy : IModifierStrategy
{
    public bool CanHandle(object modifier)
    {
        return modifier.GetType().GetMethod("ModifyAttack") != null;
    }

    public void HandleDefense(object modifier, ICreature target, int damage)
    {
        modifier.GetType().GetMethod("ModifyTakeDamage")?.Invoke(modifier, [target, damage]);
    }
}