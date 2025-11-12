namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

public abstract class Creature : ICreature
{
    public string Name { get; }

    public int CurrentHealth { get; set; }

    public int CurrentAttack { get; set; }

    public bool CanAttack => CurrentHealth > 0 && CurrentAttack > 0;

    public bool CanBeTargeted => CurrentHealth > 0;

    protected Creature(string name, int attack, int health)
    {
        Name = name;
        CurrentAttack = attack;
        CurrentHealth = health;
    }

    public virtual void Attack(ICreature target)
    {
        if (!CanAttack || !target.CanBeTargeted) return;
        target.TakeDamage(CurrentAttack);
    }

    public virtual void TakeDamage(int damage)
    {
        if (damage <= 0 || !CanBeTargeted) return;
        CurrentHealth = Math.Max(0, CurrentHealth - damage);
    }
}