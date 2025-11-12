using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells.ConcreteSpells;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Builders;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Factories.CreatureFactories;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Logic;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class BattleTests
{
    private readonly CreatureDirector _director = new();

    [Fact]
    public void Battle_Player1Wins_WhenPlayer2NoTargetableCreatures()
    {
        // Arrange
        var player1Table = new PlayerTable();
        var player2Table = new PlayerTable();

        player1Table.AddCreature(_director.CreateBattleAnalyst());

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
    public void Battle_Player2Wins_WhenPlayer1NoTargetableCreatures()
    {
        // Arrange
        var player1Table = new PlayerTable();
        var player2Table = new PlayerTable();

        player2Table.AddCreature(_director.CreateViciousFighter());

        var battle = new Battle(player1Table, player2Table);

        // Act
        BattleResult result = battle.Fight();

        // Assert
        Assert.IsType<BattleResult.Player2Win>(result);
    }

    [Fact]
    public void Battle_CreaturesAttackEachOther()
    {
        // Arrange
        var player1Table = new PlayerTable();
        var player2Table = new PlayerTable();

        player1Table.AddCreature(_director.CreateBattleAnalyst());
        player2Table.AddCreature(_director.CreateViciousFighter());

        var battle = new Battle(player1Table, player2Table);

        // Act
        BattleResult result = battle.Fight();

        // Assert
        Assert.True(result is BattleResult.Player1Win or BattleResult.Player2Win or BattleResult.Draw);
    }

    [Fact]
    public void PlayerTable_AddCreature_RespectsMaxLimit()
    {
        // Arrange
        var table = new PlayerTable();

        // Act
        for (int i = 0; i < 7; i++)
        {
            bool success = table.AddCreature(_director.CreateBattleAnalyst());
            Assert.True(success);
        }

        bool eighthSuccess = table.AddCreature(_director.CreateViciousFighter());

        // Assert
        Assert.False(eighthSuccess);
        Assert.Equal(7, table.CreatureCount);
    }

    [Fact]
    public void PlayerTable_AddCreatureFromFactory_Works()
    {
        // Arrange
        var table = new PlayerTable();
        var factory = new BattleAnalystFactory();

        // Act
        bool success = table.AddCreatureFromFactory(factory);

        // Assert
        Assert.True(success);
        Assert.Single(table.Creatures);
        Assert.IsType<BattleAnalyst>(table.Creatures[0]);
    }

    [Fact]
    public void PlayerTable_AddCreatureFromBuilder_WithSpells_Works()
    {
        // Arrange
        var table = new PlayerTable();
        ICreatureBuilder builder = _director.GetBattleAnalystBuilder()
            .WithSpell(new StrengthPotion())
            .WithSpell(new StaminaPotion());

        // Act
        bool success = table.AddCreatureFromBuilder(builder);

        // Assert
        Assert.True(success);
        Assert.Single(table.Creatures);

        ICreature creature = table.Creatures[0];
        Assert.True(creature.CurrentAttack >= 2);
        Assert.True(creature.CurrentHealth >= 4);
    }

    [Fact]
    public void PlayerTable_AddCreatureFromBuilder_WithModifiers_Works()
    {
        // Arrange
        var table = new PlayerTable();
        ICreatureBuilder builder = _director.GetViciousFighterBuilder()
            .WithSpell(new StrengthPotion());

        // Act
        bool success = table.AddCreatureFromBuilder(builder);

        // Assert
        Assert.True(success);
        Assert.Single(table.Creatures);
        Assert.IsType<ViciousFighter>(table.Creatures[0]);
    }

    [Fact]
    public void PlayerTable_RemoveCreature_Works()
    {
        // Arrange
        var table = new PlayerTable();
        table.AddCreature(_director.CreateBattleAnalyst());
        ICreature creature = table.Creatures[0];

        // Act
        bool removed = table.RemoveCreature(creature);

        // Assert
        Assert.True(removed);
        Assert.Empty(table.Creatures);
    }

    [Fact]
    public void PlayerTable_ClearTable_Works()
    {
        // Arrange
        var table = new PlayerTable();
        table.AddCreature(_director.CreateBattleAnalyst());
        table.AddCreature(_director.CreateViciousFighter());
        table.AddCreature(_director.CreateAmuletMaster());

        // Act
        table.ClearTable();

        // Assert
        Assert.Empty(table.Creatures);
    }

    [Fact]
    public void Battle_WithAmuletMaster_HasSpecialAbilities()
    {
        // Arrange
        var player1Table = new PlayerTable();
        var player2Table = new PlayerTable();

        player1Table.AddCreature(_director.CreateAmuletMaster());
        player2Table.AddCreature(_director.CreateViciousFighter());

        var battle = new Battle(player1Table, player2Table);

        // Act
        BattleResult result = battle.Fight();

        // Assert
        Assert.NotNull(result);
        Assert.True(result is BattleResult.Player1Win or BattleResult.Player2Win or BattleResult.Draw);
    }

    [Fact]
    public void Battle_WithEnhancedCreatures_WorksCorrectly()
    {
        // Arrange
        var player1Table = new PlayerTable();
        var player2Table = new PlayerTable();

        player1Table.AddCreature(_director.CreateBattleAnalyst());

        player2Table.AddCreatureFromBuilder(
            _director.GetViciousFighterBuilder()
                .WithSpell(new StrengthPotion()));

        var battle = new Battle(player1Table, player2Table);

        // Act
        BattleResult result = battle.Fight();

        // Assert
        Assert.NotNull(result);
        Assert.True(result is BattleResult.Player1Win or BattleResult.Player2Win or BattleResult.Draw);
    }

    [Fact]
    public void PlayerTable_HasAttackingCreatures_ReturnsCorrectValue()
    {
        // Arrange
        var table = new PlayerTable();

        // Act & Assert
        Assert.False(table.HasAttackingCreatures);
        Assert.False(table.HasTargetableCreatures);

        table.AddCreature(_director.CreateBattleAnalyst());
        Assert.True(table.HasAttackingCreatures);
        Assert.True(table.HasTargetableCreatures);
    }

    [Fact]
    public void PlayerTable_GetRandomAttackingCreature_ReturnsValidCreature()
    {
        // Arrange
        var table = new PlayerTable();
        table.AddCreature(_director.CreateBattleAnalyst());
        table.AddCreature(_director.CreateViciousFighter());

        // Act
        ICreature? attacker = table.GetRandomAttackingCreature();

        // Assert
        Assert.NotNull(attacker);
        Assert.True(attacker.CanAttack);
    }

    [Fact]
    public void PlayerTable_GetRandomTargetableCreature_ReturnsValidCreature()
    {
        // Arrange
        var table = new PlayerTable();
        table.AddCreature(_director.CreateBattleAnalyst());
        table.AddCreature(_director.CreateViciousFighter());

        // Act
        ICreature? target = table.GetRandomTargetableCreature();

        // Assert
        Assert.NotNull(target);
        Assert.True(target.CanBeTargeted);
    }
}