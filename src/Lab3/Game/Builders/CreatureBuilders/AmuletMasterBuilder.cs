using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.ConcreteModifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Factories.CreatureFactories;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Builders.CreatureBuilders;

public class AmuletMasterBuilder : ICreatureBuilder
{
    private readonly ModifierApplicator _modifierApplicator;
    private readonly SpellApplicator _spellApplicator;
    private ICreature _creature;

    public AmuletMasterBuilder()
    {
        var amuletMasterFactory = new AmuletMasterFactory();
        _creature = amuletMasterFactory.Create();
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
        _creature = _modifierApplicator.ActivateModifier(_creature, typeof(MagicShieldModifier));
        _creature = _modifierApplicator.ActivateModifier(_creature, typeof(DoubleStrikeModifier));

        return _creature;
    }

    public IModifierApplicator GetModifierApplicator()
    {
        return _modifierApplicator;
    }

    public ISpellApplicator GetSpellApplicator()
    {
        return _spellApplicator;
    }
}