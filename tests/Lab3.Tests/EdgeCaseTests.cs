using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.ConcreteModifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Logic;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class EdgeCaseTests
{
    [Fact]
    public void Creature_ZeroAttack_CannotAttack()
    {
        // Arrange
        var creature = new Creature("Zero Attack", 0, 5);

        // Assert
        Assert.False(creature.CanAttack);
        Assert.True(creature.CanBeTargeted);
    }

    [Fact]
    public void Creature_ZeroHealth_Dead()
    {
        // Arrange
        var creature = new Creature("Dead", 5, 0);

        // Assert
        Assert.False(creature.CanAttack);
        Assert.False(creature.CanBeTargeted);
    }

    [Fact]
    public void Battle_MaxTurns_EndsWithDraw()
    {
        // Arrange
        var player1Table = new PlayerTable();
        var player2Table = new PlayerTable();

        player1Table.AddCustomCreature("Tank1", 1, 100);
        player2Table.AddCustomCreature("Tank2", 1, 100);

        var battle = new Battle(player1Table, player2Table);

        // Act
        BattleResult result = battle.Fight();

        // Assert
        Assert.IsType<BattleResult.Draw>(result);
    }

    [Fact]
    public void PlayerTable_ModifierLimit_Enforced()
    {
        // Arrange
        var table = new PlayerTable();
        object[] modifiers =
        [
            new MagicShieldModifier(),
            new MagicShieldModifier(),
            new MagicShieldModifier() // third modifier - should fail
        ];

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            table.AddCustomCreature("Test", 1, 1, modifiers));
    }
}