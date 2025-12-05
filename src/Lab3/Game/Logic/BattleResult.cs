namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Logic;

public abstract record BattleResult
{
    private BattleResult() { }

    public sealed record Player1Win(string Reason) : BattleResult;

    public sealed record Player2Win(string Reason) : BattleResult;

    public sealed record Draw(string Reason) : BattleResult;

    public sealed record ContinueBattle : BattleResult;
}