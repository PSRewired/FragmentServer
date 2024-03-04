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

        if (character.PlayerAccount != null)
        {
            MapPlayer(info, character.PlayerAccount);
        }

        return character;
    }

    [MapProperty(nameof(CharacterInfo.SaveSlot), nameof(Character.SaveSlotId))]
    [MapProperty(nameof(CharacterInfo.Level), nameof(Character.CurrentLevel))]
    [MapProperty(nameof(CharacterInfo.Greeting), nameof(Character.GreetingMessage))]
    [MapProperty(nameof(CharacterInfo.ModelId), nameof(Character.FullModelId))]
    private static partial void MapBaseProperties(CharacterInfo info, Character character);

    [MapProperty(nameof(CharacterInfo.OnlineStatueCounter), nameof(CharacterStats.OnlineTreasures))]
    [MapProperty(nameof(CharacterInfo.OfflineStatueCounter), nameof(CharacterStats.AverageFieldLevel))]
    private static partial void MapStats(CharacterInfo stats, CharacterStats characterStats);

    private static partial void MapPlayer(CharacterInfo info, PlayerAccount player);
}
