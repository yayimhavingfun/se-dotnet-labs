using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;

public interface ISpellApplicator
{
    void ActivateSpell(ISpell spell, ICreature creature);
}