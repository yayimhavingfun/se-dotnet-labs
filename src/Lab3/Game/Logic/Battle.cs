namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Logic;

public class Battle
{
    private readonly PlayerTable _player1Table;
    private readonly PlayerTable _player2Table;

    public Battle(PlayerTable player1Table, PlayerTable player2Table)
    {
        _player1Table = player1Table;
        _player2Table = player2Table;
    }

    public BattleResult Fight()
    {
        int turn = 0;

        while (true)
        {
            BattleResult result = ProcessTurn(turn);
            if (result is not BattleResult.ContinueBattle)
                return result;

            turn++;

            if (turn > 100)
                return new BattleResult.Draw("Maximum turns reached");
        }
    }

    private BattleResult ProcessTurn(int turn)
    {
        if (turn % 2 == 0)
        {
            return ProcessPlayerAttack(
                _player1Table,
                _player2Table,
                "Player 1 has attacking creatures but Player 2 has no targetable creatures");
        }
        else
        {
            return ProcessPlayerAttack(
                _player2Table,
                _player1Table,
                "Player 2 has attacking creatures but Player 1 has no targetable creatures");
        }
    }

    private BattleResult ProcessPlayerAttack(PlayerTable attackerTable, PlayerTable defenderTable, string winReason)
    {
        TableCreature? attacker = attackerTable.GetRandomAttackingCreature();
        TableCreature? defender = defenderTable.GetRandomTargetableCreature();

        if (attacker != null && defender != null)
        {
            ProcessAttack(attacker, defender);
            return new BattleResult.ContinueBattle();
        }

        if (attacker != null && defender == null)
        {
            return new BattleResult.Player1Win(winReason);
        }

        if (attacker == null && defender != null)
        {
            return new BattleResult.ContinueBattle();
        }

        return new BattleResult.Draw("Both players have no attacking creatures and no targetable creatures");
    }

    private void ProcessAttack(TableCreature attacker, TableCreature defender)
    {
        attacker.ModifierApplicator.ModifyAttack(attacker.Creature, defender.Creature);

        if (attacker.Creature.CanAttack && defender.Creature.CanBeTargeted)
        {
            int damage = attacker.Creature.CurrentAttack;

            defender.ModifierApplicator.ModifyTakeDamage(defender.Creature, damage);

            if (damage > 0)
            {
                defender.Creature.TakeDamage(damage);
            }
        }
    }
}