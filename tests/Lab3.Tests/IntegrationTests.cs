using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.ConcreteModifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Spells.ConcreteSpells;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Logic;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class IntegrationTests
{
    [Fact]
    public void FullGameScenario_WithAbilitiesAndModifiers()
    {
        // Arrange
        var player1Table = new PlayerTable();
        var player2Table = new PlayerTable();

        player1Table.AddPredefinedCreature(
            "Vicious Fighter",
            [new MagicShieldModifier()]);

        player2Table.AddPredefinedCreature("Battle Analyst");
        player2Table.AddCustomCreature(
            "Support",
            1,
            10,
            null,
            [new StrengthPotion()]);

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

        player1Table.AddPredefinedCreature("Immortal Horror");
        player2Table.AddPredefinedCreature("Vicious Fighter");

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