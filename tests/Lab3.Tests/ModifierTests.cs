using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.ConcreteModifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.Strategies;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class ModifierTests
{
    [Fact]
    public void MagicShieldModifier_BlocksFirstDamage()
    {
        // Arrange
        var creature = new Creature("Test", 3, 10);
        var modifier = new MagicShieldModifier();

        // Act
        modifier.ModifyTakeDamage(creature, 5);

        // Assert
        Assert.Equal(10, creature.CurrentHealth); // damage blocked
    }

    [Fact]
    public void MagicShieldModifier_SecondDamage_NotBlocked()
    {
        // Arrange
        var creature = new Creature("Test", 3, 10);
        var modifier = new MagicShieldModifier();
        modifier.ModifyTakeDamage(creature, 5); // first damage - blocked

        // Act
        modifier.ModifyTakeDamage(creature, 3); // second damage

        // Assert
        Assert.Equal(7, creature.CurrentHealth); // damage applied
    }

    [Fact]
    public void DoubleStrikeModifier_AttacksTwice()
    {
        // Arrange
        var attacker = new Creature("Attacker", 2, 10);
        var target = new Creature("Target", 1, 10);
        var modifier = new DoubleStrikeModifier();

        // Act
        modifier.ModifyAttack(attacker, target);

        // Assert
        Assert.Equal(6, target.CurrentHealth); // 10 - 2 - 2 = 6
    }

    [Fact]
    public void ModifierApplicator_AddAndApplyModifiers_WorksCorrectly()
    {
        // Arrange
        var applicator = new ModifierApplicator(new AttackStrategy(), new DefenseStrategy());
        var magicShield = new MagicShieldModifier();
        var target = new Creature("Target", 2, 10);

        // Act
        applicator.AddModifier(magicShield);
        applicator.ModifyTakeDamage(target, 5);

        // Assert
        Assert.Equal(10, target.CurrentHealth); // damage blocked by shield
    }
}