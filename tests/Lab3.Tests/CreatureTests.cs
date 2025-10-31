using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class CreatureTests
{
    [Fact]
    public void Creature_Attack_WhenBothCanFight_DealsDamage()
    {
        // Arrange
        var attacker = new Creature("Attacker", 2, 5);
        var target = new Creature("Target", 1, 5);

        // Act
        attacker.Attack(target);

        // Assert
        Assert.Equal(3, target.CurrentHealth);
    }

    [Fact]
    public void Creature_Attack_WhenTargetDead_DoesNothing()
    {
        // Arrange
        var attacker = new Creature("Attacker", 2, 5);
        var target = new Creature("Target", 1, 0);

        // Act
        attacker.Attack(target);

        // Assert
        Assert.Equal(0, target.CurrentHealth);
    }

    [Fact]
    public void Creature_Attack_WhenAttackerDead_DoesNothing()
    {
        // Arrange
        var attacker = new Creature("Attacker", 2, 0);
        var target = new Creature("Target", 1, 5);

        // Act
        attacker.Attack(target);

        // Assert
        Assert.Equal(5, target.CurrentHealth);
    }

    [Fact]
    public void Creature_TakeDamage_ReducesHealth()
    {
        // Arrange
        var creature = new Creature("Test", 3, 10);

        // Act
        creature.TakeDamage(3);

        // Assert
        Assert.Equal(7, creature.CurrentHealth);
    }

    [Fact]
    public void Creature_TakeDamage_WhenDead_DoesNothing()
    {
        // Arrange
        var creature = new Creature("Test", 3, 0);

        // Act
        creature.TakeDamage(3);

        // Assert
        Assert.Equal(0, creature.CurrentHealth);
    }

    [Fact]
    public void Creature_TakeDamage_NegativeDamage_DoesNothing()
    {
        // Arrange
        var creature = new Creature("Test", 3, 10);

        // Act
        creature.TakeDamage(-5);

        // Assert
        Assert.Equal(10, creature.CurrentHealth);
    }
}