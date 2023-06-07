using Fragment.NetSlum.Core.Constants;

namespace Fragment.NetSlum.Core.Extensions;

public static class CharacterExtensions
{
    public static string ToColorCode(this CharacterColor color) => color switch
    {
        CharacterColor.Red => "rd",
        CharacterColor.Blue => "bl",
        CharacterColor.Yellow => "yl",
        CharacterColor.Green => "gr",
        CharacterColor.Brown => "br",
        CharacterColor.Pink => "pp",
        _ => throw new ArgumentOutOfRangeException(nameof(color), color, "Invalid color specified")
    };

    public static string GetClassName(this CharacterClass cls) => cls switch
    {
        CharacterClass.TwinBlade => "Twin Blade",
        CharacterClass.BladeMaster => "Blade Master",
        CharacterClass.HeavyBlade => "Heavy Blade",
        CharacterClass.AxeHeavy => "Heavy Axe",
        CharacterClass.LongArm => "Long Arm",
        CharacterClass.WaveMaster => "Wave Master",
        _ => throw new ArgumentOutOfRangeException(nameof(cls), cls, null)
    };
}
