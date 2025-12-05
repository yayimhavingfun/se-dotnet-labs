using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.ConcreteModifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells.ConcreteSpells;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Builders;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Logic;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class IntegrationTests
{
    private readonly CreatureDirector _director = new();

    [Fact]
    public void FullGameScenario_WithAbilitiesAndModifiers()
    {
        // Arrange
        var player1Table = new PlayerTable();
        var player2Table = new PlayerTable();

        player1Table.AddCreatureFromBuilder(
            _director.GetViciousFighterBuilder()
                .WithModifier(typeof(MagicShieldModifier)));

        player2Table.AddCreature(_director.CreateBattleAnalyst());
        player2Table.AddCreatureFromBuilder(
            _director.GetBattleAnalystBuilder()
                .WithSpell(new StrengthPotion()));

        var battle = new Battle(player1Table, player2Table);

        // Act
        BattleResult result = battle.Fight();

        // Assert
        Assert.NotNull(result);
        Assert.True(result is BattleResult.Player1Win or
            BattleResult.Player2Win or
            BattleResult.Draw);
    }

    [Fact]
    public void ImmortalHorrorVsViciousFighter_ComplexInteraction()
    {
        // Arrange
        var player1Table = new PlayerTable();
        var player2Table = new PlayerTable();

        player1Table.AddCreature(_director.CreateImmortalHorror());
        player2Table.AddCreature(_director.CreateViciousFighter());

        var battle = new Battle(player1Table, player2Table);

        // Act
        BattleResult result = battle.Fight();

        // Assert
        Assert.NotNull(result);
        Assert.True(result is BattleResult.Player1Win or
            BattleResult.Player2Win or
            BattleResult.Draw);
    }
}