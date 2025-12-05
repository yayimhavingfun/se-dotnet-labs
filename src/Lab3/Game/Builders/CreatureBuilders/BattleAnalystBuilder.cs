using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Builders.CreatureBuilders;

public class BattleAnalystBuilder : ICreatureBuilder
{
    private readonly ModifierApplicator _modifierApplicator;
    private readonly SpellApplicator _spellApplicator;
    private readonly BattleAnalyst _creature;

    public BattleAnalystBuilder()
    {
        _creature = new BattleAnalyst();
        _modifierApplicator = new ModifierApplicator();
        _spellApplicator = new SpellApplicator(_modifierApplicator);
    }

    public ICreatureBuilder WithModifier(object modifier)
    {
        _modifierApplicator.AddModifier(_creature, modifier);
        return this;
    }

    public ICreatureBuilder WithSpell(ISpell spell)
    {
        _spellApplicator.AddSpell(_creature, spell);
        return this;
    }

    public ICreature Build()
    {
        return _creature;
    }
}