using System.Collections.Generic;
using System.Linq;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;
using Fragment.NetSlum.Persistence.Extensions;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;
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

    public PlayersController(ITcpServer gameServer, FragmentContext database)
    {
        _gameServer = gameServer;
        _database = database;
    }

    /// <summary>
    /// Returns information on all players
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public PagedResult<PlayerInfo> GetAllPlayers(int page = 1, int pageSize = 50, string? characterName = null)
    {
        IQueryable<Character> characters = _database.Characters
            .AsNoTracking()
            .Include(g => g.CharacterStats);

        if (!string.IsNullOrWhiteSpace(characterName))
        {
            characters = characters.Where(c =>
                EF.Functions.Collate(
                    EF.Functions.Like(c.CharacterName, $"%{characterName}%"), $"utf8mb4_0900_ai_ci"));
        }

        var characterCount = characters.Count();

        var results = characters.Paginate(page, pageSize)
            .Select(r => CharacterMapper.Map(r))
            .ToList();

        return new PagedResult<PlayerInfo>(page, pageSize, characterCount, results);
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

        return player == null ? null : CharacterMapper.Map(player);
    }

    /// <summary>
    /// Returns character information for all characters associated with an account ID
    /// </summary>
    /// <param name="accountId"></param>
    /// <response code="200"></response>
    /// <response code="204">Account does not exist or no data could be found</response>
    [HttpGet("account/{accountId:int}")]
    public IEnumerable<PlayerInfo> GetAccountPlayerInfos(int accountId)
    {
        var players = _database.Characters
            .AsNoTracking()
            .Include(p => p.CharacterStats)
            .Where(p => p.PlayerAccountId == accountId);

        foreach (var player in players)
        {
            yield return CharacterMapper.Map(player);
        }
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
            .Where(p => p.CharacterId == characterId)
            .OrderByDescending(p => p.CreatedAt);

        foreach (var stats in playerStats)
        {
            yield return CharacterMapper.Map(stats);
        }
    }
}
