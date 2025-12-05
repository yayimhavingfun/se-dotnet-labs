using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Builders;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Factories;
using System.Collections.ObjectModel;
using System.Security.Cryptography;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Logic;

public class PlayerTable
{
    private const int MaxCreatures = 7;
    private readonly List<ICreature> _creatures = [];

    public ReadOnlyCollection<ICreature> Creatures => _creatures.AsReadOnly();

    public int CreatureCount => _creatures.Count;

    private bool IsFull => _creatures.Count >= MaxCreatures;

    public bool AddCreature(ICreature creature)
    {
        if (IsFull) return false;
        _creatures.Add(creature);
        return true;
    }

    // Метод для добавления через фабрику
    public bool AddCreatureFromFactory(ICreatureFactory factory)
    {
        if (IsFull) return false;

        try
        {
            ICreature creature = factory.Create();
            _creatures.Add(creature);
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    public bool AddCreatureFromBuilder(ICreatureBuilder builder)
    {
        if (IsFull) return false;

        try
        {
            ICreature creature = builder.Build();
            _creatures.Add(creature);
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    public bool RemoveCreature(ICreature creature)
    {
        return _creatures.Remove(creature);
    }

    public void ClearTable()
    {
        _creatures.Clear();
    }

    public ICreature? GetRandomAttackingCreature()
    {
        var attacking = GetAttackingCreatures().ToList();
        if (attacking.Count == 0) return null;

        byte[] randomBytes = new byte[4];
        RandomNumberGenerator.Fill(randomBytes);
        uint randomValue = BitConverter.ToUInt32(randomBytes, 0);
        int randomIndex = (int)(randomValue % (uint)attacking.Count);

        return attacking[randomIndex];
    }

    public ICreature? GetRandomTargetableCreature()
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

    private IEnumerable<ICreature> GetAttackingCreatures()
    {
        return _creatures.Where(c => c.CanAttack);
    }

    private IEnumerable<ICreature> GetTargetableCreatures()
    {
        return _creatures.Where(c => c.CanBeTargeted);
    }
}