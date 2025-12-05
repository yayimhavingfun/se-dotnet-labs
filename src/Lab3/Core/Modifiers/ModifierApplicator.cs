using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using System.Reflection;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;

public class ModifierApplicator : IModifierApplicator
{
    private readonly Dictionary<ICreature, List<object>> _creatureModifiers = new();

    public void AddModifier(ICreature creature, object modifier)
    {
        if (!_creatureModifiers.TryGetValue(creature, out List<object>? value))
        {
            value = [];
            _creatureModifiers[creature] = value;
        }

        value.Add(modifier);
    }

    public void RemoveModifier(ICreature creature, ICreature modifier)
    {
        if (_creatureModifiers.TryGetValue(creature, out List<object>? creatureModifier))
        {
            creatureModifier.Remove(modifier);
        }
    }

    public void ClearModifiers(ICreature creature)
    {
        if (_creatureModifiers.TryGetValue(creature, out List<object>? modifier))
        {
            modifier.Clear();
        }
    }

    public ICreature ActivateModifier(ICreature creature, Type modifierType)
    {
        if (!_creatureModifiers.TryGetValue(creature, out List<object>? creatureModifier))
            return creature;

        object? modifier = creatureModifier.FirstOrDefault(m => m.GetType() == modifierType);
        if (modifier == null) return creature;
        _creatureModifiers[creature].Remove(modifier);
        return ApplyModifier(creature, modifier);
    }

    public ICreature ApplyModifier(ICreature creature, object modifier)
    {
        ConstructorInfo? constructor = modifier.GetType().GetConstructor([typeof(ICreature)]);
        return constructor != null
            ? (ICreature)constructor.Invoke([creature])
            : creature;
    }
}