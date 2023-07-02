using AutoMapper;
using Fragment.NetSlum.Persistence.Entities;
using Fragment.NetSlum.Server.Api.Models;

namespace Fragment.NetSlum.Server.Mappings;

public class CharacterProfile : Profile
{
    public CharacterProfile()
    {
        CreateMap<Character, PlayerInfo>()
            .ForMember(x => x.CharacterName, y => y.MapFrom(z => z.CharacterName))
            .ForMember(x => x.Class, y => y.MapFrom(z => z.Class))
            .ForMember(x => x.Level, y => y.MapFrom(z => z.CurrentLevel))
            .ForMember(x => x.Greeting, y => y.MapFrom(z => z.GreetingMessage))
            .ForMember(x => x.ModelId, y => y.MapFrom(z => z.FullModelId))
            .ForMember(x => x.GuildId, y => y.MapFrom(z => z.GuildId))
            .ForMember(x => x.CurrentHp, y => y.MapFrom(z => z.CharacterStats.CurrentHp))
            .ForMember(x => x.CurrentSp, y => y.MapFrom(z => z.CharacterStats.CurrentSp))
            .ForMember(x => x.CurrentGp, y => y.MapFrom(z => z.CharacterStats.CurrentGp))
            .ForMember(x => x.GoldCoinCount, y => y.MapFrom(z => z.CharacterStats.GoldAmount))
            .ForMember(x => x.SilverCoinCount, y => y.MapFrom(z => z.CharacterStats.SilverAmount))
            .ForMember(x => x.BronzeCoinCount, y => y.MapFrom(z => z.CharacterStats.BronzeAmount))
            .ForMember(x => x.OnlineTreasures, y => y.MapFrom(z => z.CharacterStats.OnlineTreasures))
            .ForMember(x => x.AverageFieldLevel, y => y.MapFrom(z => z.CharacterStats.AverageFieldLevel))
        ;

        CreateMap<CharacterStatHistory, PlayerStats>()
            .ForMember(x => x.CurrentHp, y => y.MapFrom(z => z.CurrentHp))
            .ForMember(x => x.CurrentSp, y => y.MapFrom(z => z.CurrentSp))
            .ForMember(x => x.CurrentGp, y => y.MapFrom(z => z.CurrentGp))
            .ForMember(x => x.GoldCoinCount, y => y.MapFrom(z => z.GoldAmount))
            .ForMember(x => x.SilverCoinCount, y => y.MapFrom(z => z.SilverAmount))
            .ForMember(x => x.BronzeCoinCount, y => y.MapFrom(z => z.BronzeAmount))
            .ForMember(x => x.OnlineTreasures, y => y.MapFrom(z => z.OnlineTreasures))
            .ForMember(x => x.AverageFieldLevel, y => y.MapFrom(z => z.AverageFieldLevel))
            .ForMember(x => x.UpdatedAt, y => y.MapFrom(z => z.CreatedAt))
        ;
    }
}
