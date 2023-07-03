using System.Linq;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Server.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fragment.NetSlum.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatsController
{
    private readonly FragmentContext _database;

    public StatsController(FragmentContext database)
    {
        _database = database;
    }

    /// <summary>
    /// Returns statistical data about this server instance
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public ServerStats GetServerStats()
    {
        return new ServerStats
        {
            TotalBbsPosts = _database.BbsThreads.Count(),
            ActiveGuilds = _database.Guilds.Count(),
            RegisteredAccounts = _database.PlayerAccounts.Count(),
            RegisteredCharacters = _database.Characters.Count(),
        };
    }
}
