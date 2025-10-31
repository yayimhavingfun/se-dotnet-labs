using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells.Strategies;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;

public interface ISpellApplicator
{
    void AddSpellStrategy(ISpellStrategy strategy);

    void RemoveSpellStrategy(ISpellStrategy strategy);

    void StoreSpell(ISpell spell);

    void RemoveSpell(ISpell spell);

    void ApplySpell(ISpell spell, ICreature creature);
}