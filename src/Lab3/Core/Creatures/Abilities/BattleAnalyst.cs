namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures.Abilities;

public class BattleAnalyst : CreatureDecorator
{
    public BattleAnalyst(ICreature creature) : base(creature) { }

    public override void Attack(ICreature target)
    {
        CurrentAttack += 2;
        base.Attack(target);
    }
}