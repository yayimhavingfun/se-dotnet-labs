using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells.Strategies;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;

public class SpellApplicator : ISpellApplicator
{
    private readonly List<ISpellStrategy> _strategies = [];
    private readonly List<ISpell> _spells = [];

    public void AddSpellStrategy(ISpellStrategy strategy)
    {
        _strategies.Add(strategy);
    }

    public void RemoveSpellStrategy(ISpellStrategy strategy)
    {
        _strategies.Remove(strategy);
    }

    public void StoreSpell(ISpell spell)
    {
        _spells.Add(spell);
    }

    public void RemoveSpell(ISpell spell)
    {
        _spells.Remove(spell);
    }

    public void ApplySpell(ISpell spell, ICreature creature)
    {
        if (_spells.Contains(spell))
        {
            ISpellStrategy? strategy = _strategies.FirstOrDefault(s => s.CanHandle(spell));
            strategy?.Handle(spell, creature);
        }
    }

    public IEnumerable<ISpell> GetSpells()
    {
        return _spells.AsReadOnly();
    }
}