namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

public class MimicChest : Creature
{
    public MimicChest() : base("Mimic Chest", 1, 1) { }

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