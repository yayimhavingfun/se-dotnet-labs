using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.Strategies;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;

public class ModifierApplicator : IModifierApplicator
{
    private readonly List<IModifierStrategy> _strategies;
    private readonly Dictionary<Type, List<object>> _modifiers = new();

    public ModifierApplicator(params IModifierStrategy[] strategies)
    {
        _strategies = strategies.ToList();
    }

    public void AddModifier(object modifier)
    {
        IModifierStrategy? strategy = _strategies.FirstOrDefault(s => s.CanHandle(modifier));
        if (strategy != null)
        {
            Type type = strategy.GetType();
            if (!_modifiers.ContainsKey(type))
                _modifiers[type] = [];
            _modifiers[type].Add(modifier);
        }
    }

    public void RemoveModifier(object modifier)
    {
        foreach (KeyValuePair<Type, List<object>> kvp in _modifiers)
        {
            kvp.Value.Remove(modifier);
        }
    }

    public void ModifyAttack(ICreature attacker, ICreature target)
    {
        IEnumerable<AttackStrategy> attackStrategies = _strategies.OfType<AttackStrategy>();
        foreach (AttackStrategy strategy in attackStrategies)
        {
            if (_modifiers.TryGetValue(strategy.GetType(), out List<object>? modifiers))
            {
                foreach (object modifier in modifiers)
                    strategy.HandleAttack(modifier, attacker, target);
            }
        }
    }

    public void ModifyTakeDamage(ICreature target, int damage)
    {
        IEnumerable<DefenseStrategy> defenseStrategies = _strategies.OfType<DefenseStrategy>();
        foreach (DefenseStrategy strategy in defenseStrategies)
        {
            if (_modifiers.TryGetValue(strategy.GetType(), out List<object>? modifiers))
            {
                foreach (object modifier in modifiers)
                    strategy.HandleDefense(modifier, target, damage);
            }
        }
    }
}