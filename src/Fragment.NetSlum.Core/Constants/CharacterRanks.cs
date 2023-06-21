using System.Text.RegularExpressions;

namespace Fragment.NetSlum.Core.Constants;

public static partial class CharacterRanks
{
    // ReSharper disable InconsistentNaming
    public enum RankCategory : byte
    {
        Level = 1,
        HP = 2,
        SP = 3,
        GP = 4,
        OnlineTreasures = 5,
        AverageFieldLevel = 6,
        GoldCoin = 7,
        SilverCoin = 8,
        BronzeCoin = 9,
    }
    // ReSharper enable InconsistentNaming

    public static string GetCategoryName(this RankCategory category)
    {
        return FormatRegex().Replace(StripCharsRegex().Replace(category.ToString(), "$1 $2"), "$1 $2");
    }

    [GeneratedRegex("(\\p{Ll})(\\P{Ll})")]
    private static partial Regex FormatRegex();

    [GeneratedRegex("(\\P{Ll})(\\P{Ll}\\p{Ll})")]
    private static partial Regex StripCharsRegex();
}
