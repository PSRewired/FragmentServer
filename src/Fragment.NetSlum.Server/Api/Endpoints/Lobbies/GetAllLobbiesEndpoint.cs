using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Fragment.NetSlum.Networking.Stores;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;

namespace Fragment.NetSlum.Server.Api.Endpoints.Lobbies;

public class GetAllLobbiesEndpoint : Endpoint<EmptyRequest, IEnumerable<Lobby>>
{
    private readonly ChatLobbyStore _lobbyStore;

    public GetAllLobbiesEndpoint(ChatLobbyStore lobbyStore)
    {
        _lobbyStore = lobbyStore;
    }

    public override void Configure()
    {
        Get("/lobbies");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Returns a list of all available lobbies and player metadata for each player within the lobby";
            s.Description = "Returns a list of all available lobbies and player metadata for each player within the lobby";
        });
    }

    public override Task<IEnumerable<Lobby>> ExecuteAsync(EmptyRequest req, CancellationToken ct)
    {
        return Task.FromResult(_lobbyStore.ChatLobbies.Select(LobbyMapper.Map));
    }
}
