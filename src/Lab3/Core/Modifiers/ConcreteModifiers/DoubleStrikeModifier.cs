using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.ConcreteModifiers;

public class DoubleStrikeModifier : ICreature
{
    private readonly ICreature _creature;
    private bool _isActive = true;

    public DoubleStrikeModifier(ICreature creature)
    {
        _creature = creature;
    }

    public string Name => _creature.Name;

    public int CurrentAttack { get => _creature.CurrentAttack; set => _creature.CurrentAttack = value; }

    public int CurrentHealth { get => _creature.CurrentHealth; set => _creature.CurrentHealth = value; }

    public bool CanAttack => _creature.CanAttack;

    public bool CanBeTargeted => _creature.CanBeTargeted;

    public void Attack(ICreature target)
    {
        _creature.Attack(target);

        if (target.CanBeTargeted && _isActive && _creature.CanAttack)
        {
            _creature.Attack(target);
            _isActive = false;
        }
    }

    public void TakeDamage(int damage)
    {
        _creature.TakeDamage(damage);
    }
}