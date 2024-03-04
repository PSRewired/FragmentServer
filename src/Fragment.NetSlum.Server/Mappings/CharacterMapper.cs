// ReSharper disable RedundantVerbatimPrefix
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Persistence.Entities;
using Fragment.NetSlum.Server.Api.Models;
using Riok.Mapperly.Abstractions;

namespace Fragment.NetSlum.Server.Mappings;

[Mapper]
public static partial class CharacterMapper
{
    public static PlayerInfo Map(Character character)
    {
        var obj = MapCharacter(character);
        obj.AvatarId = GetModelFileName(character);

        return obj;
    }

    [MapProperty(nameof(CharacterStatHistory.GoldAmount), nameof(PlayerStats.GoldCoinCount))]
    [MapProperty(nameof(CharacterStatHistory.SilverAmount), nameof(PlayerStats.SilverCoinCount))]
    [MapProperty(nameof(CharacterStatHistory.BronzeAmount), nameof(PlayerStats.BronzeCoinCount))]
    [MapProperty(nameof(CharacterStatHistory.CreatedAt), nameof(PlayerStats.UpdatedAt))]
    public static partial PlayerStats Map(CharacterStatHistory history);

    [MapProperty(nameof(Character.CurrentLevel), nameof(PlayerInfo.Level))]
    [MapProperty(nameof(Character.GreetingMessage), nameof(PlayerInfo.Greeting))]
    [MapProperty(nameof(Character.FullModelId), nameof(PlayerInfo.ModelId))]
    [MapProperty(nameof(Character.PlayerAccountId), nameof(PlayerInfo.AccountId))]
    [MapProperty(nameof(@Character.CharacterStats.CurrentHp), nameof(PlayerInfo.CurrentHp))]
    [MapProperty(nameof(@Character.CharacterStats.CurrentSp), nameof(PlayerInfo.CurrentSp))]
    [MapProperty(nameof(@Character.CharacterStats.CurrentGp), nameof(PlayerInfo.CurrentGp))]
    [MapProperty(nameof(@Character.CharacterStats.GoldAmount), nameof(PlayerInfo.GoldCoinCount))]
    [MapProperty(nameof(@Character.CharacterStats.SilverAmount), nameof(PlayerInfo.SilverCoinCount))]
    [MapProperty(nameof(@Character.CharacterStats.BronzeAmount), nameof(PlayerInfo.BronzeCoinCount))]
    [MapProperty(nameof(@Character.CharacterStats.OnlineTreasures), nameof(PlayerInfo.OnlineTreasures))]
    [MapProperty(nameof(@Character.CharacterStats.AverageFieldLevel), nameof(PlayerInfo.AverageFieldLevel))]
    private static partial PlayerInfo MapCharacter(Character character);

    private static string GetModelFileName(Character character)
    {
        var classLetter = character.Class.ToString().ToLower()[0];

        return $"xf{classLetter}{GetModelNumber(character.FullModelId)}{GetModelType(character.FullModelId)}_{GetModelColor(character.FullModelId).ToColorCode()}";
    }

    private static char GetModelClass(uint modelNumber)
    {
        return ((CharacterClass) (modelNumber & 0x0F)).ToString().ToLower()[0];
    }

    private static int GetModelNumber(uint modelNumber)
    {
        return (int) ((modelNumber >> 4 & 0x0F) + 1);
    }

    private static char GetModelType(uint modelNumber)
    {
        return (char) (((modelNumber >> 12) & 0x0F) + 0x41);
    }

    private static CharacterColor GetModelColor(uint modelNumber)
    {
        return (CharacterColor) ((modelNumber >> 8) & 0x0F);
    }
}
