using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures.Abilities;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.ConcreteModifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Builders;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class CreatureBuilderTests
{
    [Fact]
    public void CreatureBuilder_FromCustom_CreatesCorrectCreature()
    {
        // Arrange
        var builder = new CreatureBuilder();

        // Act
        (ICreature creature, _, _) = builder.FromCustom("Custom", 3, 7).Build();

        // Assert
        Assert.Equal("Custom", creature.Name);
        Assert.Equal(3, creature.CurrentAttack);
        Assert.Equal(7, creature.CurrentHealth);
    }

    [Fact]
    public void CreatureBuilder_FromPredefined_CreatesCorrectCreature()
    {
        // Arrange
        var builder = new CreatureBuilder();

        // Act
        (ICreature creature, IModifierApplicator _, Core.Spells.ISpellApplicator _) =
            builder.FromPredefined("Vicious Fighter").Build();

        // Assert
        Assert.Equal("Vicious Fighter", creature.Name);
        Assert.IsType<ViciousFighter>(creature);
    }

    [Fact]
    public void CreatureBuilder_WithModifier_AddsModifier()
    {
        // Arrange
        var builder = new CreatureBuilder();
        var shield = new MagicShieldModifier();

        // Act
        (_, IModifierApplicator modifierApplicator, _) = builder.FromCustom("Test", 1, 1)
            .WithModifier(shield)
            .Build();

        // Assert
        var creature = new Creature("Test", 1, 5);
        modifierApplicator.ModifyTakeDamage(creature, 3);
        Assert.Equal(5, creature.CurrentHealth); // damage blocked
    }
}