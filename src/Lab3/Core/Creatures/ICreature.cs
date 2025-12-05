namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

public interface ICreature
{
    string Name { get; }

    int CurrentAttack { get; set; }

    int CurrentHealth { get; set; }

    bool CanAttack { get; }

    bool CanBeTargeted { get; }

    void Attack(ICreature target);

    void TakeDamage(int damage);
}