namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures.Abilities;

public class CreatureDecorator : ICreature
{
    private readonly ICreature _creature;

    protected CreatureDecorator(ICreature creature)
    {
        _creature = creature;
    }

    public virtual string Name => _creature.Name;

    public virtual int CurrentAttack
    {
        get => _creature.CurrentAttack;
        set => _creature.CurrentAttack = value;
    }

    public virtual int CurrentHealth
    {
        get => _creature.CurrentHealth;
        set => _creature.CurrentHealth = value;
    }

    public virtual bool CanAttack => _creature.CanAttack;

    public virtual bool CanBeTargeted => _creature.CanBeTargeted;

    public virtual void Attack(ICreature target)
    {
        _creature.Attack(target);
    }

    public virtual void TakeDamage(int damage)
    {
        _creature.TakeDamage(damage);
    }
}