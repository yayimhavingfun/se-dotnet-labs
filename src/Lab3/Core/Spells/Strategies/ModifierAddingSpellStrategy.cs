using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Spells.Strategies;

public class ModifierAddingSpellStrategy : ISpellStrategy
{
    private readonly IModifierApplicator _modifierApplicator;

    public ModifierAddingSpellStrategy(IModifierApplicator modifierApplicator)
    {
        _modifierApplicator = modifierApplicator;
    }

    public bool CanHandle(ISpell spell)
    {
        return spell is IModifierSpell;
    }

    public void Handle(ISpell spell, ICreature creature)
    {
        if (spell is IModifierSpell modifierSpell)
        {
            object modifier = modifierSpell.CreateModifier();
            _modifierApplicator.AddModifier(modifier);
        }
    }
}