using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.ConcreteModifiers;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class ModifierTests
{
    [Fact]
    public void MagicShieldModifier_BlocksFirstDamage()
    {
        // Arrange
        var creature = new BasicCreature("Test", 3, 10);
        var modifier = new MagicShieldModifier(creature);

        // Act
        modifier.TakeDamage(5);

        // Assert
        Assert.Equal(10, creature.CurrentHealth);
    }

    [Fact]
    public void MagicShieldModifier_SecondDamage_NotBlocked()
    {
        // Arrange
        var creature = new BasicCreature("Test", 3, 10);
        var modifier = new MagicShieldModifier(creature);
        modifier.TakeDamage(5);

        // Act
        modifier.TakeDamage(3);

        // Assert
        Assert.Equal(7, creature.CurrentHealth);
    }

    [Fact]
    public void DoubleStrikeModifier_AttacksTwice()
    {
        // Arrange
        var attacker = new BasicCreature("Attacker", 2, 10);
        var target = new BasicCreature("Target", 1, 10);
        var modifier = new DoubleStrikeModifier(attacker);

        // Act
        modifier.Attack(target);

        // Assert
        Assert.Equal(6, target.CurrentHealth); // 10 - 2 - 2 = 6
    }

    [Fact]
    public void ModifierApplicator_AddAndApplyModifiers_WorksCorrectly()
    {
        // Arrange
        var applicator = new ModifierApplicator();
        var baseCreature = new BasicCreature("Target", 2, 10);

        // Act
        applicator.AddModifier(baseCreature, new MagicShieldModifier(baseCreature));
        ICreature modifiedCreature = applicator.ActivateModifier(baseCreature, typeof(MagicShieldModifier));
        modifiedCreature.TakeDamage(5);

        // Assert
        Assert.Equal(10, modifiedCreature.CurrentHealth);
    }
}