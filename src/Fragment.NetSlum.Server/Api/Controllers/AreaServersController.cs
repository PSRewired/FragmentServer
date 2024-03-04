using System.Collections.Generic;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.Server.Mappings;
using Fragment.NetSlum.TcpServer;
using Microsoft.AspNetCore.Mvc;

namespace Fragment.NetSlum.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AreaServersController : ControllerBase
{
    private readonly ITcpServer _gameServer;

    public AreaServersController(ITcpServer gameServer)
    {
        _gameServer = gameServer;
    }

    /// <summary>
    /// Returns information on all online area servers
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IEnumerable<AreaServerStatus> GetOnlineAreaServers()
    {
        foreach (var client in _gameServer.Sessions)
        {
            if (client is not FragmentTcpSession { IsAreaServer: true } session)
            {
                continue;
            }

            if (session.AreaServerInfo == null)
            {
                continue;
            }

            yield return AreaServerMapper.Map(session.AreaServerInfo);
        }
    }
}
