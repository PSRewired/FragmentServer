using System.Collections.Generic;
using AutoMapper;
using Fragment.NetSlum.Networking.Stores;
using Fragment.NetSlum.Server.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fragment.NetSlum.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LobbiesController : ControllerBase
{
    private readonly ChatLobbyStore _lobbyStore;
    private readonly IMapper _mapper;

    public LobbiesController(ChatLobbyStore lobbyStore, IMapper mapper)
    {
        _lobbyStore = lobbyStore;
        _mapper = mapper;
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
            yield return _mapper.Map<Lobby>(lobby);
        }
    }
}
