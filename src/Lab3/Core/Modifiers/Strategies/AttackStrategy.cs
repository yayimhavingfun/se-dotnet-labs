using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.Strategies;

public class AttackStrategy : IModifierStrategy
{
    public bool CanHandle(object modifier)
    {
        return modifier.GetType().GetMethod("ModifyAttack") != null;
    }

    public void HandleAttack(object modifier, ICreature attacker, ICreature target)
    {
        modifier.GetType().GetMethod("ModifyAttack")?.Invoke(modifier, [attacker, target]);
    }
}