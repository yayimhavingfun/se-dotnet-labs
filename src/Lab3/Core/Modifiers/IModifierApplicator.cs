using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;

public interface IModifierApplicator
{
    void ModifyAttack(ICreature attacker, ICreature target);

    void ModifyTakeDamage(ICreature target, int damage);

    void AddModifier(object modifier);

    void RemoveModifier(object modifier);
}