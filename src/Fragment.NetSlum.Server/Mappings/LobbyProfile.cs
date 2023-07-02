using System.Collections.Generic;
using AutoMapper;
using Fragment.NetSlum.Networking.Models;
using Fragment.NetSlum.Server.Api.Models;

namespace Fragment.NetSlum.Server.Mappings;

public class LobbyProfile : Profile
{
    public LobbyProfile()
    {
        CreateMap<ChatLobbyModel, Lobby>()
            .ForMember(x => x.Id, y => y.MapFrom(z => z.LobbyId))
            .ForMember(x => x.Name, y => y.MapFrom(z => z.LobbyName))
            .ForMember(x => x.Type, y => y.MapFrom(z => z.LobbyType))
            .ForMember(x => x.PlayerCount, y => y.MapFrom(z => z.PlayerCount))
            .ForMember(x => x.Players,
                y => y.MapFrom((src, dst, z, context) =>
                    context.Mapper.Map<ICollection<ChatLobbyPlayer>, ICollection<LobbyPlayer>>(src.GetPlayers())))
        ;

        CreateMap<ChatLobbyPlayer, LobbyPlayer>()
            .ForMember(x => x.CharacterId, y => y.MapFrom(z => z.PlayerCharacterId))
            .ForMember(x => x.CharacterName, y => y.MapFrom(z => z.PlayerName))
            .ForMember(x => x.JoinedAt, y => y.MapFrom(z => z.JoinedAt))
        ;
    }
}
