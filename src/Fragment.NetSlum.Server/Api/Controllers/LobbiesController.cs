using System.Collections.Generic;
using Fragment.NetSlum.Networking.Stores;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace Fragment.NetSlum.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LobbiesController : ControllerBase
{
    private readonly ChatLobbyStore _lobbyStore;

    public LobbiesController(ChatLobbyStore lobbyStore)
    {
        _lobbyStore = lobbyStore;
    }

    /// <summary>
    /// Returns a list of all available lobbies and player metadata for each player within the lobby
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<Lobby> GetAllLobbies()
    {
        foreach (var lobby in _lobbyStore.ChatLobbies)
        {
            yield return LobbyMapper.Map(lobby);
        }
    }
}
