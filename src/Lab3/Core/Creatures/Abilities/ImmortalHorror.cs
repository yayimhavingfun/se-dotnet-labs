namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures.Abilities;

public class ImmortalHorror : CreatureDecorator
{
    public ImmortalHorror(ICreature creature) : base(creature) { }

    private bool _hasRevived = false;

    public override void TakeDamage(int damage)
    {
        if (!CanBeTargeted) return;

        bool willDie = CurrentHealth <= damage;

        if (willDie && !_hasRevived)
        {
            CurrentHealth = 1;
            _hasRevived = true;
            return;
        }

        base.TakeDamage(damage);
    }
}