using Fragment.NetSlum.Core.Models;
using Fragment.NetSlum.Persistence.Entities;
using Riok.Mapperly.Abstractions;

namespace Fragment.NetSlum.Server.Mappings;

[Mapper]
public static partial class CharacterInfoMapper
{
    public static Character MapOrCreate(CharacterInfo info, Character? character)
    {
        character ??= new Character
        {
            CharacterName = info.CharacterName,
            GreetingMessage = info.Greeting,
            FullModelId = info.ModelId,
            Class = info.Class,
            CurrentLevel = info.Level,
        };

        MapBaseProperties(info, character);
        MapStats(info, character.CharacterStats);

        return character;
    }

    [MapProperty(nameof(CharacterInfo.SaveSlot), nameof(Character.SaveSlotId))]
    [MapProperty(nameof(CharacterInfo.Level), nameof(Character.CurrentLevel))]
    [MapProperty(nameof(CharacterInfo.Greeting), nameof(Character.GreetingMessage))]
    [MapProperty(nameof(CharacterInfo.ModelId), nameof(Character.FullModelId))]
    [MapperIgnoreTarget(nameof(Character.Id))]
    [MapperIgnoreTarget(nameof(Character.PlayerAccountId))]
    [MapperIgnoreTarget(nameof(Character.PlayerAccount))]
    [MapperIgnoreTarget(nameof(Character.CharacterStats))]
    [MapperIgnoreTarget(nameof(Character.IpLogs))]
    [MapperIgnoreTarget(nameof(Character.Guild))]
    [MapperIgnoreTarget(nameof(Character.GuildId))]
    [MapperIgnoreTarget(nameof(Character.CreatedAt))]
    [MapperIgnoreTarget(nameof(Character.LastLoginAt))]
    private static partial void MapBaseProperties(CharacterInfo info, Character character);

    [MapProperty(nameof(CharacterInfo.OnlineStatueCounter), nameof(CharacterStats.OnlineTreasures))]
    [MapProperty(nameof(CharacterInfo.OfflineStatueCounter), nameof(CharacterStats.AverageFieldLevel))]
    [MapperIgnoreTarget(nameof(CharacterStats.Id))]
    [MapperIgnoreTarget(nameof(CharacterStats.CharacterId))]
    [MapperIgnoreTarget(nameof(CharacterStats.Character))]
    [MapperIgnoreTarget(nameof(CharacterStats.GoldAmount))]
    [MapperIgnoreTarget(nameof(CharacterStats.SilverAmount))]
    [MapperIgnoreTarget(nameof(CharacterStats.BronzeAmount))]
    [MapperIgnoreTarget(nameof(CharacterStats.CreatedAt))]
    [MapperIgnoreTarget(nameof(CharacterStats.UpdatedAt))]
    private static partial void MapStats(CharacterInfo stats, CharacterStats characterStats);

}
