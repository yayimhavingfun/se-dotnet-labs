namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures.Abilities;

public class ViciousFighter : CreatureDecorator
{
    public ViciousFighter(ICreature creature) : base(creature) { }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (CanBeTargeted)
            CurrentAttack *= 2;
    }
}