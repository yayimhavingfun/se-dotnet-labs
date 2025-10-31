using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Builders;
using System.Security.Cryptography;
using ICreature = Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures.ICreature;
using IModifierApplicator = Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.IModifierApplicator;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Logic;

public class PlayerTable
{
    private const int MaxCreatures = 7;
    private const int MaxModifiersPerCreature = 2;
    private const int MaxSpellsPerCreature = 2;
    private readonly List<TableCreature> _creatures = [];

    public IReadOnlyList<TableCreature> Creatures => _creatures.AsReadOnly();

    public int CreatureCount => _creatures.Count;

    private bool IsFull => _creatures.Count >= MaxCreatures;

    public bool AddPredefinedCreature(string name, object[]? modifiers = null, ISpell[]? spells = null)
    {
        if (IsFull) return false;

        if (modifiers?.Length > MaxModifiersPerCreature)
            throw new InvalidOperationException($"Cannot add more than {MaxModifiersPerCreature} modifiers");

        if (spells?.Length > MaxSpellsPerCreature)
            throw new InvalidOperationException($"Cannot add more than {MaxSpellsPerCreature} spells");

        try
        {
            CreatureBuilder builder = new CreatureBuilder().FromPredefined(name);

            if (modifiers != null)
            {
                foreach (object modifier in modifiers)
                {
                    builder.WithModifier(modifier);
                }
            }

            if (spells != null)
            {
                foreach (ISpell spell in spells)
                {
                    builder.WithSpell(spell);
                }
            }

            (ICreature Creature, IModifierApplicator ModifierApplicator, ISpellApplicator SpellApplicator) result =
                builder.Build();
            _creatures.Add(new TableCreature(result.Creature, result.ModifierApplicator, result.SpellApplicator));
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    public bool AddCustomCreature(
        string name,
        int attack,
        int health,
        object[]? modifiers = null,
        ISpell[]? spells = null)
    {
        if (IsFull) return false;

        if (modifiers?.Length > MaxModifiersPerCreature)
            throw new InvalidOperationException($"Cannot add more than {MaxModifiersPerCreature} modifiers");

        if (spells?.Length > MaxSpellsPerCreature)
            throw new InvalidOperationException($"Cannot add more than {MaxSpellsPerCreature} spells");

        CreatureBuilder builder = new CreatureBuilder().FromCustom(name, attack, health);

        if (modifiers != null)
        {
            foreach (object modifier in modifiers)
            {
                builder.WithModifier(modifier);
            }
        }

        if (spells != null)
        {
            foreach (ISpell spell in spells)
            {
                builder.WithSpell(spell);
            }
        }

        (ICreature Creature, IModifierApplicator ModifierApplicator, ISpellApplicator SpellApplicator) result =
            builder.Build();
        _creatures.Add(new TableCreature(result.Creature, result.ModifierApplicator, result.SpellApplicator));
        return true;
    }

    public bool RemoveCreature(ICreature creature)
    {
        TableCreature? tableCreature = _creatures.FirstOrDefault(c => c.Creature == creature);
        return tableCreature != null && _creatures.Remove(tableCreature);
    }

    public bool RemoveCreatureAt(int index)
    {
        if (index >= 0 && index < _creatures.Count)
        {
            _creatures.RemoveAt(index);
            return true;
        }

        return false;
    }

    public void ClearTable()
    {
        _creatures.Clear();
    }

    public TableCreature? GetRandomAttackingCreature()
    {
        var attacking = GetAttackingCreatures().ToList();
        if (attacking.Count == 0) return null;

        byte[] randomBytes = new byte[4];
        RandomNumberGenerator.Fill(randomBytes);
        uint randomValue = BitConverter.ToUInt32(randomBytes, 0);
        int randomIndex = (int)(randomValue % (uint)attacking.Count);

        return attacking[randomIndex];
    }

    public TableCreature? GetRandomTargetableCreature()
    {
        var targetable = GetTargetableCreatures().ToList();
        if (targetable.Count == 0) return null;

        byte[] randomBytes = new byte[4];
        RandomNumberGenerator.Fill(randomBytes);
        uint randomValue = BitConverter.ToUInt32(randomBytes, 0);
        int randomIndex = (int)(randomValue % (uint)targetable.Count);

        return targetable[randomIndex];
    }

    public bool HasAttackingCreatures => GetAttackingCreatures().Any();

    public bool HasTargetableCreatures => GetTargetableCreatures().Any();

    private IEnumerable<TableCreature> GetAttackingCreatures()
    {
        return _creatures.Where(c => c.Creature.CanAttack);
    }

    private IEnumerable<TableCreature> GetTargetableCreatures()
    {
        return _creatures.Where(c => c.Creature.CanBeTargeted);
    }
}