using System.Collections.Generic;
using System.Linq;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Extensions;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuildsController
{
    private readonly FragmentContext _database;

    public GuildsController(FragmentContext database)
    {
        _database = database;
    }

    /// <summary>
    /// Returns information about all available guilds
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public PagedResult<GuildInfo> GetAllGuilds(int page = 1, int pageSize = 50)
    {
        var guilds = _database.Guilds
            .AsNoTracking()
            .Include(g => g.Stats)
            .Include(g => g.Members)
            .OrderBy(g => g.Id)
            .Paginate(page, pageSize);

        var guildCount = _database.Guilds.Count();

        return new PagedResult<GuildInfo>(page, pageSize, guildCount, guilds.Select(g => GuildMapper.Map(g)).ToList());
    }

    /// <summary>
    /// Returns detailed information about the given guild ID
    /// </summary>
    /// <param name="guildId"></param>
    /// <returns></returns>
    [HttpGet("{guildId:int}")]
    public GuildInfo? GetGuildInfo(int guildId)
    {
        var guild = _database.Guilds
            .AsNoTracking()
            .Include(g => g.Stats)
            .Include(g => g.Members)
            .FirstOrDefault(g => g.Id == guildId);

        return guild == null ? null : GuildMapper.Map(guild);
    }

    /// <summary>
    /// Returns a list of all current members of the given guild ID
    /// </summary>
    /// <param name="guildId"></param>
    /// <returns></returns>
    [HttpGet("{guildId:int}/members")]
    public IEnumerable<PlayerInfo> GetGuildMembers(int guildId)
    {
        var members = _database.Characters
            .AsNoTracking()
            .Include(p => p.CharacterStats)
            .Where(p => p.GuildId == guildId);

        foreach (var member in members)
        {
            yield return CharacterMapper.Map(member);
        }
    }
}
