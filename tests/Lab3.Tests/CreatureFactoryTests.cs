using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Creatures.Abilities;
using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers;
using Itmo.ObjectOrientedProgramming.Lab3.Game.Factories.CreatureFactories;
using Xunit;

namespace Itmo.ObjectOrientedProgramming.Lab3.Tests;

public class CreatureFactoryTests
{
    [Fact]
    public void ViciousFighterFactory_CreatesViciousFighter()
    {
        // Arrange
        var factory = new ViciousFighterFactory();

        // Act
        (ICreature creature, _, _) = factory.Create();

        // Assert
        Assert.IsType<ViciousFighter>(creature);
        Assert.Equal("Vicious Fighter", creature.Name);
    }

    [Fact]
    public void AmuletMasterFactory_HasModifiers()
    {
        // Arrange
        var factory = new AmuletMasterFactory();

        // Act
        (ICreature creature, IModifierApplicator modifierApplicator, _) = factory.Create();

        // Assert
        Assert.Equal("Amulet Master", creature.Name);

        var testCreature = new Creature("Test", 1, 10);
        modifierApplicator.ModifyTakeDamage(testCreature, 5);

        Assert.Equal(10, testCreature.CurrentHealth);
    }
}