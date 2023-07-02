using AutoMapper;
using Fragment.NetSlum.Persistence.Entities;
using Fragment.NetSlum.Server.Api.Models;

namespace Fragment.NetSlum.Server.Mappings;

public class GuildProfile : Profile
{
    public GuildProfile()
    {
        CreateMap<Guild, GuildInfo>()
            .ForMember(x => x.Name, y => y.MapFrom(z => z.Name))
            .ForMember(x => x.Comment, y => y.MapFrom(z => z.Comment))
            .ForMember(x => x.LeaderId, y => y.MapFrom(z => z.LeaderId))
            .ForMember(x => x.CreatedAt, y => y.MapFrom(z => z.CreatedAt))
            .ForMember(x => x.Name, y => y.MapFrom(z => z.Name))
            .ForMember(x => x.MemberCount, y => y.MapFrom(z => z.Members.Count))
            .ForMember(x => x.GoldAmount, y => y.MapFrom(z => z.Stats.GoldAmount))
            .ForMember(x => x.SilverAmount, y => y.MapFrom(z => z.Stats.SilverAmount))
            .ForMember(x => x.BronzeAmount, y => y.MapFrom(z => z.Stats.BronzeAmount))
            .ForMember(x => x.CurrentGp, y => y.MapFrom(z => z.Stats.CurrentGp))
            .ForMember(x => x.LastUpdatedAt, y => y.MapFrom(z => z.Stats.UpdatedAt))
        ;
    }
}
