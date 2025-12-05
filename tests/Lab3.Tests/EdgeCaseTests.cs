using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Logic;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class EdgeCaseTests
{
    [Fact]
    public void Creature_ZeroAttack_CannotAttack()
    {
        // Arrange
        var creature = new BasicCreature("Zero Attack", 0, 5);

        // Assert
        Assert.False(creature.CanAttack);
        Assert.True(creature.CanBeTargeted);
    }

    [Fact]
    public void Creature_ZeroHealth_Dead()
    {
        // Arrange
        var creature = new BasicCreature("Dead", 5, 0);

        // Assert
        Assert.False(creature.CanAttack);
        Assert.False(creature.CanBeTargeted);
    }

    [Fact]
    public void PlayerTable_GetRandomAttackingCreature_EmptyTable_ReturnsNull()
    {
        // Arrange
        var table = new PlayerTable();

        // Act
        ICreature? creature = table.GetRandomAttackingCreature();

        // Assert
        Assert.Null(creature);
    }
}