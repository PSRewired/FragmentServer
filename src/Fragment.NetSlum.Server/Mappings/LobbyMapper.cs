using System.Collections.Generic;
using System.Linq;
using Fragment.NetSlum.Networking.Models;
using Fragment.NetSlum.Server.Api.Models;
using Riok.Mapperly.Abstractions;

namespace Fragment.NetSlum.Server.Mappings;

[Mapper]
public static partial class LobbyMapper
{
    public static Lobby Map(ChatLobbyModel model)
    {
        var obj = MapLobby(model);

        obj.Players = Players(model);

        return obj;
    }

    [MapProperty(nameof(ChatLobbyModel.LobbyId), nameof(Lobby.Id))]
    [MapProperty(nameof(ChatLobbyModel.LobbyName), nameof(Lobby.Name))]
    [MapProperty(nameof(ChatLobbyModel.LobbyType), nameof(Lobby.Type))]
    [MapProperty(nameof(ChatLobbyModel.PlayerCount), nameof(Lobby.PlayerCount))]
    public static partial Lobby MapLobby(ChatLobbyModel lobby);

    [MapProperty(nameof(ChatLobbyPlayer.PlayerCharacterId), nameof(LobbyPlayer.CharacterId))]
    [MapProperty(nameof(ChatLobbyPlayer.PlayerName), nameof(LobbyPlayer.CharacterName))]
    public static partial LobbyPlayer MapPlayer(ChatLobbyPlayer player);

    private static ICollection<LobbyPlayer> Players(ChatLobbyModel model) => model.GetPlayers().Select(MapPlayer).ToArray();
}
