using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Builders.CreatureBuilders;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Factories.CreatureFactories;

namespace Itmo.ObjectOrientedProgramming.Lab3.Game.Builders;

public class CreatureDirector
{
    public ICreature CreateBattleAnalyst() => new BattleAnalystFactory().Create();

    public ICreature CreateViciousFighter() => new ViciousFighterFactory().Create();

    public ICreature CreateAmuletMaster() => new AmuletMasterFactory().Create();

    public ICreature CreateMimicChest() => new MimicChestFactory().Create();

    public ICreature CreateImmortalHorror() => new ImmortalHorrorFactory().Create();

    public BattleAnalystBuilder GetBattleAnalystBuilder() => new();

    public ViciousFighterBuilder GetViciousFighterBuilder() => new();

    public AmuletMasterBuilder GetAmuletMasterBuilder() => new();

    public MimicChestBuilder GetMimicChestBuilder() => new();

    public ImmortalHorrorBuilder GetImmortalHorrorBuilder() => new();
}