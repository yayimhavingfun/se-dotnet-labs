namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;

public class BattleAnalyst : Creature
{
    public BattleAnalyst() : base("Battle Analyst", 2, 4) { }

    public override void Attack(ICreature target)
    {
        CurrentAttack += 2;
        base.Attack(target);
    }
}