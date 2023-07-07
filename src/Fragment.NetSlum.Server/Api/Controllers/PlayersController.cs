using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Extensions;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.TcpServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly ITcpServer _gameServer;
    private readonly FragmentContext _database;
    private readonly IMapper _mapper;

    public PlayersController(ITcpServer gameServer, FragmentContext database, IMapper mapper)
    {
        _gameServer = gameServer;
        _database = database;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns information on all players
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public PagedResult<PlayerInfo> GetAllPlayers(int page = 1, int pageSize = 50, string? characterName = null)
    {
        var guilds = _database.Characters
            .AsNoTracking()
            .Include(g => g.CharacterStats)
            .OrderBy(g => g.Id)
            .Paginate(page, pageSize);

        if (!string.IsNullOrWhiteSpace(characterName))
        {
            guilds = guilds.Where(c => c.CharacterName.Contains(characterName));
        }

        var guildCount = _database.Guilds.Count();

        return new PagedResult<PlayerInfo>(page, pageSize, guildCount, _mapper.Map<List<PlayerInfo>>(guilds.ToList()));
    }

    /// <summary>
    /// Returns information on all online players
    /// </summary>
    /// <returns></returns>
    [HttpGet("online")]
    public IEnumerable<Client> GetOnlinePlayers()
    {
        foreach (var client in _gameServer.Sessions)
        {
            if (client is not FragmentTcpSession session || session.IsAreaServer)
            {
                continue;
            }

            yield return Client.FromSession(session);
        }
    }

    /// <summary>
    /// Returns information about the given player ID
    /// </summary>
    /// <param name="characterId"></param>
    /// <response code="200"></response>
    /// <response code="204">Player does not exist or no data could be found</response>
    [HttpGet("{characterId:int}")]
    public PlayerInfo? GetPlayerInfo(int characterId)
    {
        var player = _database.Characters
            .AsNoTracking()
            .Include(p => p.CharacterStats)
            .FirstOrDefault(p => p.Id == characterId);

        return _mapper.Map<PlayerInfo>(player);
    }

    /// <summary>
    /// Returns historical statistics for the given player ID
    /// </summary>
    /// <param name="characterId"></param>
    /// <response code="200"></response>
    /// <response code="204">Player does not exist or no data could be found</response>
    [HttpGet("{characterId:int}/stats")]
    public IEnumerable<PlayerStats> GetPlayerStatHistory(int characterId)
    {
        var playerStats = _database.CharacterStatHistory
            .AsNoTracking()
            .Where(p => p.Id == characterId)
            .OrderByDescending(p => p.CreatedAt);

        foreach (var stats in playerStats)
        {
            yield return _mapper.Map<PlayerStats>(stats);
        }
    }
}
