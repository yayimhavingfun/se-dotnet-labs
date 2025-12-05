using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;

public class SpellApplicator : ISpellApplicator
{
    private readonly Dictionary<ICreature, List<ISpell>> _creatureSpells = new();
    private readonly ModifierApplicator _modifierApplicator;

    public SpellApplicator(ModifierApplicator modifierApplicator)
    {
        _modifierApplicator = modifierApplicator;
    }

    public void AddSpell(ICreature creature, ISpell spell)
    {
        if (!_creatureSpells.TryGetValue(creature, out List<ISpell>? value))
        {
            value = [];
            _creatureSpells[creature] = value;
        }

        value.Add(spell);
    }

    public void RemoveSpell(ICreature creature, ISpell spell)
    {
        if (_creatureSpells.TryGetValue(creature, out List<ISpell>? creatureSpells))
        {
            creatureSpells.Remove(spell);
        }
    }

    public void ClearSpells(ICreature creature)
    {
        if (_creatureSpells.TryGetValue(creature, out List<ISpell>? spells))
        {
            spells.Clear();
        }
    }

    public void ActivateSpell(ISpell spell, ICreature creature)
    {
        switch (spell)
        {
            case IStatSpell statSpell:
                ApplyStatSpell(creature, statSpell);
                break;
            case IModifierSpell modifierSpell:
                ApplyModifierSpell(creature, modifierSpell);
                break;
            default:
                break;
        }
    }

    private void ApplyStatSpell(ICreature creature, IStatSpell spell)
    {
        spell.ChangeStats(creature);
    }

    private void ApplyModifierSpell(ICreature creature, IModifierSpell spell)
    {
        object modifierType = spell.CreateModifier();
        _modifierApplicator.AddModifier(creature, modifierType);
    }
}