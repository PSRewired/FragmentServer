using AutoMapper;
using Fragment.NetSlum.Networking.Models;
using Fragment.NetSlum.Server.Api.Models;

namespace Fragment.NetSlum.Server.Mappings;

public class AreaServerProfile : Profile
{
    public AreaServerProfile()
    {
        CreateMap<AreaServerInformation, AreaServerStatus>()
            .ForMember(x => x.Name, y => y.MapFrom(z => z.ServerName))
            .ForMember(x => x.Level, y => y.MapFrom(z => z.Level))
            .ForMember(x => x.CurrentPlayerCount, y => y.MapFrom(z => z.CurrentPlayerCount))
            .ForMember(x => x.State, y => y.MapFrom(z => z.State))
            .ForMember(x => x.OnlineSince, y => y.MapFrom(z => z.ActiveSince))
        ;
    }
}
