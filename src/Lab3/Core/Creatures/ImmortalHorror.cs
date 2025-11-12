namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

public class ImmortalHorror : Creature
{
    public ImmortalHorror() : base("Immortal Horror", 4, 4) { }

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