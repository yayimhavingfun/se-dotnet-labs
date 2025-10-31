using Itmo.ObjectOrientedProgramming.Lab3.Core.Modifiers.ConcreteModifiers;

namespace Itmo.ObjectOrientedProgramming.Lab3.Core.Spells.ConcreteSpells;

public class ProtectionAmulet : IModifierSpell
{
    public string Name => "Protection Amulet";

    public object CreateModifier()
    {
        return new MagicShieldModifier();
    }
}