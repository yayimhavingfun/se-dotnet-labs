namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

public class ViciousFighter : Creature
{
    public ViciousFighter() : base("Vicious Fighter", 1, 6) { }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (CanBeTargeted)
            CurrentAttack *= 2;
    }
}