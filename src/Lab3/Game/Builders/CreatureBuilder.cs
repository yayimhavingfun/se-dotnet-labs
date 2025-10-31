using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.Strategies;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells.Strategies;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Factories;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Factories.CreatureFactories;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Builders;

public class CreatureBuilder
{
    private readonly Dictionary<string, ICreatureFactory> _creatureFactories = new()
    {
        ["Battle Analyst"] = new BattleAnalystFactory(),
        ["Vicious Fighter"] = new ViciousFighterFactory(),
        ["Mimic Chest"] = new MimicChestFactory(),
        ["Immortal Horror"] = new ImmortalHorrorFactory(),
        ["Amulet Master"] = new AmuletMasterFactory(),
    };

    private ICreature? _baseCreature;

    private IModifierApplicator? _modifierApplicator;

    private ISpellApplicator? _spellApplicator;

    public CreatureBuilder FromPredefined(string name)
    {
        if (_creatureFactories.TryGetValue(name, out ICreatureFactory? factory))
        {
            (ICreature creature, IModifierApplicator modifierApplicator, ISpellApplicator spellApplicator) =
                factory.Create();
            _baseCreature = creature;
            _modifierApplicator = modifierApplicator;
            _spellApplicator = spellApplicator;
        }
        else
        {
            throw new ArgumentException($"Unknown creature type: {name}");
        }

        return this;
    }

    public CreatureBuilder FromCustom(string name, int attack, int health)
    {
        _baseCreature = new Creature(name, attack, health);

        _modifierApplicator = new ModifierApplicator(new AttackStrategy(), new DefenseStrategy());

        _spellApplicator = new SpellApplicator();

        _spellApplicator.AddSpellStrategy(new StatChangingSpellStrategy());
        _spellApplicator.AddSpellStrategy(new ModifierAddingSpellStrategy(_modifierApplicator));

        return this;
    }

    public CreatureBuilder WithModifier(object modifier)
    {
        if (_modifierApplicator == null)
        {
            throw new InvalidOperationException("No creature defined. Use FromPredefined() or FromCustom() first");
        }

        _modifierApplicator.AddModifier(modifier);
        return this;
    }

    public CreatureBuilder WithSpell(ISpell spell)
    {
        if (_spellApplicator == null || _baseCreature == null)
        {
            throw new InvalidOperationException("No creature defined. Use FromPredefined() or FromCustom() first.");
        }

        _spellApplicator.ApplySpell(spell, _baseCreature);
        return this;
    }

    public (ICreature Creature, IModifierApplicator ModifierApplicator, ISpellApplicator SpellApplicator) Build()
    {
        if (_baseCreature == null || _modifierApplicator == null || _spellApplicator == null)
        {
            throw new InvalidOperationException("No creature defined. Use FromPredefined() or FromCustom() first.");
        }

        return (_baseCreature, _modifierApplicator, _spellApplicator);
    }
}