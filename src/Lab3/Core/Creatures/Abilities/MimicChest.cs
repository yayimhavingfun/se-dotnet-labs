namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures.Abilities;

public class MimicChest : CreatureDecorator
{
    public MimicChest(ICreature creature) : base(creature) { }

    public override void Attack(ICreature target)
    {
        if (target.CanBeTargeted)
        {
            CurrentAttack = Math.Max(CurrentAttack, target.CurrentAttack);
            CurrentHealth = Math.Max(CurrentHealth, target.CurrentHealth);
        }

        base.Attack(target);
    }
}