using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures.Abilities;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class AbilityTests
{
    [Fact]
    public void ViciousFighter_TakeDamage_DoublesAttack()
    {
        // Arrange
        var baseCreature = new Creature("Fighter", 1, 6);
        var viciousFighter = new ViciousFighter(baseCreature);

        // Act
        viciousFighter.TakeDamage(2);

        // Assert
        Assert.Equal(2, viciousFighter.CurrentAttack);
        Assert.Equal(4, viciousFighter.CurrentHealth);
    }

    [Fact]
    public void ViciousFighter_TakeDamage_WhenDead_DoesNotDoubleAttack()
    {
        // Arrange
        var baseCreature = new Creature("Fighter", 1, 2);
        var viciousFighter = new ViciousFighter(baseCreature);

        // Act
        viciousFighter.TakeDamage(3);

        // Assert
        Assert.Equal(1, viciousFighter.CurrentAttack);
        Assert.Equal(0, viciousFighter.CurrentHealth);
    }

    [Fact]
    public void MimicChest_Attack_CopiesStats()
    {
        // Arrange
        var mimic = new MimicChest(new Creature("Mimic", 1, 1));
        var target = new Creature("Target", 5, 10);

        // Act
        mimic.Attack(target);

        // Assert
        Assert.Equal(5, mimic.CurrentAttack);
        Assert.Equal(10, mimic.CurrentHealth);
    }

    [Fact]
    public void ImmortalHorror_TakeDamage_FirstDeath_Revives()
    {
        // Arrange
        var horror = new ImmortalHorror(new Creature("Horror", 4, 4));

        // Act
        horror.TakeDamage(4);

        // Assert
        Assert.Equal(1, horror.CurrentHealth);
        Assert.True(horror.CanBeTargeted);
    }

    [Fact]
    public void ImmortalHorror_TakeDamage_SecondDeath_Dies()
    {
        // Arrange
        var horror = new ImmortalHorror(new Creature("Horror", 4, 4));
        horror.TakeDamage(4); // first death - revives

        // Act
        horror.TakeDamage(1); // second death

        // Assert
        Assert.Equal(0, horror.CurrentHealth);
        Assert.False(horror.CanBeTargeted);
    }

    [Fact]
    public void BattleAnalyst_Attack_IncreasesAttack()
    {
        // Arrange
        var analyst = new BattleAnalyst(new Creature("Analyst", 2, 4));
        var target = new Creature("Target", 1, 10);

        // Act
        analyst.Attack(target);

        // Assert
        Assert.Equal(4, analyst.CurrentAttack); // 2 + 2
        Assert.Equal(6, target.CurrentHealth); // 10 - 2
    }
}