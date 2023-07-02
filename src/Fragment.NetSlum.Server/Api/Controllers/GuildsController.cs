using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Extensions;
using Fragment.NetSlum.Server.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fragment.NetSlum.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuildsController
{
    private readonly FragmentContext _database;
    private readonly IMapper _mapper;

    public GuildsController(FragmentContext database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
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

        return new PagedResult<GuildInfo>(page, pageSize, guildCount, _mapper.Map<List<GuildInfo>>(guilds.ToList()));
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

        return _mapper.Map<GuildInfo>(guild);
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
            yield return _mapper.Map<PlayerInfo>(member);
        }
    }
}
