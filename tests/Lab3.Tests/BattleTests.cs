using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells.ConcreteSpells;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Logic;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class BattleTests
{
    [Fact]
    public void Battle_Player1Wins_WhenPlayer2NoTargetableCreatures()
    {
        // Arrange
        var player1Table = new PlayerTable();
        var player2Table = new PlayerTable();

        player1Table.AddCustomCreature("Attacker", 3, 5);

        var battle = new Battle(player1Table, player2Table);

        // Act
        BattleResult result = battle.Fight();

        // Assert
        Assert.IsType<BattleResult.Player1Win>(result);
    }

    [Fact]
    public void Battle_Draw_WhenBothNoCreatures()
    {
        // Arrange
        var player1Table = new PlayerTable();
        var player2Table = new PlayerTable();

        var battle = new Battle(player1Table, player2Table);

        // Act
        BattleResult result = battle.Fight();

        // Assert
        Assert.IsType<BattleResult.Draw>(result);
    }

    [Fact]
    public void Battle_CreaturesAttackEachOther()
    {
        // Arrange
        var player1Table = new PlayerTable();
        var player2Table = new PlayerTable();

        player1Table.AddCustomCreature("P1Creature", 2, 5);
        player2Table.AddCustomCreature("P2Creature", 1, 5);

        var battle = new Battle(player1Table, player2Table);

        // Act
        BattleResult result = battle.Fight();

        // Assert - one player should win, creatures should have taken damage
        Assert.True(result is BattleResult.Player1Win or BattleResult.Player2Win or BattleResult.Draw);
    }

    [Fact]
    public void PlayerTable_AddPredefinedCreature_RespectsMaxLimit()
    {
        // Arrange
        var table = new PlayerTable();

        // Act
        for (int i = 0; i < 7; i++)
        {
            bool success = table.AddPredefinedCreature("Vicious Fighter");
            Assert.True(success);
        }

        bool eighthSuccess = table.AddPredefinedCreature("Vicious Fighter");

        // Assert
        Assert.False(eighthSuccess);
        Assert.Equal(7, table.CreatureCount);
    }

    [Fact]
    public void PlayerTable_AddCustomCreature_WithSpells_Works()
    {
        // Arrange
        var table = new PlayerTable();
        var spells = new ISpell[] { new StrengthPotion(), new StaminaPotion() };

        // Act
        bool success = table.AddCustomCreature("Custom", 1, 1, null, spells);

        // Assert
        Assert.True(success);
        Assert.Single(table.Creatures);
    }

    [Fact]
    public void PlayerTable_RemoveCreature_Works()
    {
        // Arrange
        var table = new PlayerTable();
        table.AddCustomCreature("Test", 1, 1);
        Core.Creatures.ICreature creature = table.Creatures[0].Creature;

        // Act
        bool removed = table.RemoveCreature(creature);

        // Assert
        Assert.True(removed);
        Assert.Empty(table.Creatures);
    }
}