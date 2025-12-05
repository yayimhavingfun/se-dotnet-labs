using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class CreatureTests
{
    [Fact]
    public void Creature_Attack_WhenBothCanFight_DealsDamage()
    {
        // Arrange
        var attacker = new BasicCreature("Attacker", 2, 5);
        var target = new BasicCreature("Target", 1, 5);

        // Act
        attacker.Attack(target);

        // Assert
        Assert.Equal(3, target.CurrentHealth);
    }

    [Fact]
    public void Creature_Attack_WhenTargetDead_DoesNothing()
    {
        // Arrange
        var attacker = new BasicCreature("Attacker", 2, 5);
        var target = new BasicCreature("Target", 1, 0);

        // Act
        attacker.Attack(target);

        // Assert
        Assert.Equal(0, target.CurrentHealth);
    }

    [Fact]
    public void Creature_Attack_WhenAttackerDead_DoesNothing()
    {
        // Arrange
        var attacker = new BasicCreature("Attacker", 2, 0);
        var target = new BasicCreature("Target", 1, 5);

        // Act
        attacker.Attack(target);

        // Assert
        Assert.Equal(5, target.CurrentHealth);
    }

    [Fact]
    public void Creature_TakeDamage_ReducesHealth()
    {
        // Arrange
        var creature = new BasicCreature("Test", 3, 10);

        // Act
        creature.TakeDamage(3);

        // Assert
        Assert.Equal(7, creature.CurrentHealth);
    }

    [Fact]
    public void Creature_TakeDamage_WhenDead_DoesNothing()
    {
        // Arrange
        var creature = new BasicCreature("Test", 3, 0);

        // Act
        creature.TakeDamage(3);

        // Assert
        Assert.Equal(0, creature.CurrentHealth);
    }

    [Fact]
    public void Creature_TakeDamage_NegativeDamage_DoesNothing()
    {
        // Arrange
        var creature = new BasicCreature("Test", 3, 10);

        // Act
        creature.TakeDamage(-5);

        // Assert
        Assert.Equal(10, creature.CurrentHealth);
    }

    [Fact]
    public void Creature_Initialization_SetsCorrectProperties()
    {
        // Arrange & Act
        var creature = new BasicCreature("Warrior", 5, 10);

        // Assert
        Assert.Equal("Warrior", creature.Name);
        Assert.Equal(5, creature.CurrentAttack);
        Assert.Equal(10, creature.CurrentHealth);
        Assert.True(creature.CanAttack);
        Assert.True(creature.CanBeTargeted);
    }

    [Fact]
    public void Creature_ZeroHealth_CannotAttackOrBeTargeted()
    {
        // Arrange
        var creature = new BasicCreature("Dead", 5, 0);

        // Assert
        Assert.False(creature.CanAttack);
        Assert.False(creature.CanBeTargeted);
    }

    [Fact]
    public void Creature_ZeroAttack_CannotAttack()
    {
        // Arrange
        var creature = new BasicCreature("Weak", 0, 10);

        // Assert
        Assert.False(creature.CanAttack);
        Assert.True(creature.CanBeTargeted);
    }

    [Fact]
    public void Creature_Attack_DoesNotDamageAttacker()
    {
        // Arrange
        var attacker = new BasicCreature("Attacker", 3, 10);
        var target = new BasicCreature("Target", 2, 8);

        // Act
        attacker.Attack(target);

        // Assert
        Assert.Equal(10, attacker.CurrentHealth);
        Assert.Equal(5, target.CurrentHealth);
    }

    [Fact]
    public void Creature_TakeDamage_ZeroDamage_DoesNothing()
    {
        // Arrange
        var creature = new BasicCreature("Test", 3, 10);

        // Act
        creature.TakeDamage(0);

        // Assert
        Assert.Equal(10, creature.CurrentHealth);
    }

    [Fact]
    public void Creature_TakeDamage_ExactHealth_KillsCreature()
    {
        // Arrange
        var creature = new BasicCreature("Test", 3, 5);

        // Act
        creature.TakeDamage(5);

        // Assert
        Assert.Equal(0, creature.CurrentHealth);
        Assert.False(creature.CanAttack);
        Assert.False(creature.CanBeTargeted);
    }
}