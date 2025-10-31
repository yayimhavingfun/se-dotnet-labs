using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.Strategies;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells.Strategies;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Factories;

public abstract class CreatureFactory : ICreatureFactory
{
    public string Name { get; }

    protected int Attack { get; }

    protected int Health { get; }

    protected CreatureFactory(string name, int attack, int health)
    {
        Name = name;
        Attack = attack;
        Health = health;
    }

    public virtual (ICreature Creature, IModifierApplicator ModifierApplicator, ISpellApplicator SpellApplicator)
        Create()
    {
        var creature = new Creature(Name, Attack, Health);
        IModifierApplicator modifierApplicator = CreateModifierApplicator();
        ISpellApplicator spellApplicator = CreateSpellApplicator(modifierApplicator);

        return (creature, modifierApplicator, spellApplicator);
    }

    protected virtual IModifierApplicator CreateModifierApplicator()
    {
        return new ModifierApplicator(new AttackStrategy(), new DefenseStrategy());
    }

    protected virtual ISpellApplicator CreateSpellApplicator(IModifierApplicator modifierApplicator)
    {
        var spellApplicator = new SpellApplicator();
        spellApplicator.AddSpellStrategy(new StatChangingSpellStrategy());
        spellApplicator.AddSpellStrategy(new ModifierAddingSpellStrategy(modifierApplicator));
        return spellApplicator;
    }
}