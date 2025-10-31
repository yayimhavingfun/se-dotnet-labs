using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.ConcreteModifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells.ConcreteSpells;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class SpellTests
{
    [Fact]
    public void StrengthPotion_IncreasesAttack()
    {
        // Arrange
        var creature = new Creature("Test", 5, 10);
        var potion = new StrengthPotion();

        // Act
        potion.ChangeStats(creature);

        // Assert
        Assert.Equal(10, creature.CurrentAttack);
    }

    [Fact]
    public void StaminaPotion_IncreasesHealth()
    {
        // Arrange
        var creature = new Creature("Test", 5, 10);
        var potion = new StaminaPotion();

        // Act
        potion.ChangeStats(creature);

        // Assert
        Assert.Equal(15, creature.CurrentHealth);
    }

    [Fact]
    public void MagicMirror_SwapsStats()
    {
        // Arrange
        var creature = new Creature("Test", 5, 10);
        var mirror = new MagicMirror();

        // Act
        mirror.ChangeStats(creature);

        // Assert
        Assert.Equal(10, creature.CurrentAttack);
        Assert.Equal(5, creature.CurrentHealth);
    }

    [Fact]
    public void ProtectionAmulet_CreatesMagicShield()
    {
        // Arrange
        var amulet = new ProtectionAmulet();

        // Act
        object modifier = amulet.CreateModifier();

        // Assert
        Assert.IsType<MagicShieldModifier>(modifier);
    }
}