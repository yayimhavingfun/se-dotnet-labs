using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Builders.CreatureBuilders;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class AbilityTests
{
    [Fact]
    public void ViciousFighter_TakeDamage_DoublesAttack()
    {
        // Arrange
        var viciousFighter = new ViciousFighter();

        // Act
        viciousFighter.TakeDamage(2);

        // Assert
        Assert.Equal(2, viciousFighter.CurrentAttack); // 1 * 2 = 2
        Assert.Equal(4, viciousFighter.CurrentHealth); // 6 - 2 = 4
    }

    [Fact]
    public void ViciousFighter_TakeDamage_WhenDead_DoesNotDoubleAttack()
    {
        // Arrange
        var viciousFighter = new ViciousFighter();
        viciousFighter.TakeDamage(5);

        // Act
        viciousFighter.TakeDamage(1);

        // Assert
        Assert.Equal(2, viciousFighter.CurrentAttack);
        Assert.Equal(0, viciousFighter.CurrentHealth);
        Assert.False(viciousFighter.CanAttack);
        Assert.False(viciousFighter.CanBeTargeted);
    }

    [Fact]
    public void MimicChest_Attack_CopiesStats()
    {
        // Arrange
        var mimic = new MimicChest();
        var target = new BasicCreature("Target", 5, 10);

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
        var horror = new ImmortalHorror();

        // Act
        horror.TakeDamage(4);

        // Assert
        Assert.Equal(1, horror.CurrentHealth);
        Assert.True(horror.CanBeTargeted);
        Assert.True(horror.CanAttack);
    }

    [Fact]
    public void ImmortalHorror_TakeDamage_SecondDeath_Dies()
    {
        // Arrange
        var horror = new ImmortalHorror();
        horror.TakeDamage(4);

        // Act
        horror.TakeDamage(1);

        // Assert
        Assert.Equal(0, horror.CurrentHealth);
        Assert.False(horror.CanBeTargeted);
        Assert.False(horror.CanAttack);
    }

    [Fact]
    public void BattleAnalyst_Attack_IncreasesAttack()
    {
        // Arrange
        var analyst = new BattleAnalyst();
        var target = new BasicCreature("Target", 1, 10);

        // Act
        analyst.Attack(target);

        // Assert
        Assert.Equal(4, analyst.CurrentAttack); // 2 + 2 = 4
        Assert.Equal(6, target.CurrentHealth); // 10 - 4 = 6
    }

    [Fact]
    public void BattleAnalyst_MultipleAttacks_ContinueIncreasingAttack()
    {
        // Arrange
        var analyst = new BattleAnalyst();
        var target1 = new BasicCreature("Target1", 1, 20);
        var target2 = new BasicCreature("Target2", 1, 20);

        // Act
        analyst.Attack(target1); // +2 атаки
        analyst.Attack(target2); // +2 атаки

        // Assert
        Assert.Equal(6, analyst.CurrentAttack); // 2 + 2 + 2 = 6
        Assert.Equal(16, target1.CurrentHealth); // 20 - 4 = 16
    }

    [Fact]
    public void AmuletMaster_HasMagicShield_BlocksFirstDamage()
    {
        // Arrange
        ICreature amuletMaster = new AmuletMasterBuilder().Build();

        // Act
        amuletMaster.TakeDamage(5);

        // Assert
        Assert.Equal(2, amuletMaster.CurrentHealth);
    }

    [Fact]
    public void AmuletMaster_CanDoubleStrike_AttacksTwice()
    {
        // Arrange
        ICreature amuletMaster = new AmuletMasterBuilder().Build();
        var target = new BasicCreature("Target", 1, 15);

        // Act
        amuletMaster.Attack(target);

        // Assert
        Assert.Equal(5, target.CurrentHealth);
    }

    [Fact]
    public void BasicCreature_Attack_ReducesTargetHealth()
    {
        // Arrange
        var attacker = new BasicCreature("Attacker", 3, 10);
        var target = new BasicCreature("Target", 2, 8);

        // Act
        attacker.Attack(target);

        // Assert
        Assert.Equal(5, target.CurrentHealth); // 8 - 3 = 5
        Assert.Equal(10, attacker.CurrentHealth); // не изменилось
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
}