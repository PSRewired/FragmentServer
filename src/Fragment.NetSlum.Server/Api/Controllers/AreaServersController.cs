using System.Collections.Generic;
using AutoMapper;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Server.Api.Models;
using Fragment.NetSlum.TcpServer;
using Microsoft.AspNetCore.Mvc;

namespace Fragment.NetSlum.Server.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AreaServersController : ControllerBase
{
    private readonly ITcpServer _gameServer;
    private readonly IMapper _mapper;

    public AreaServersController(ITcpServer gameServer, IMapper mapper)
    {
        _gameServer = gameServer;
        _mapper = mapper;
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
            if (client is not FragmentTcpSession session || !session.IsAreaServer)
            {
                continue;
            }

            yield return _mapper.Map<AreaServerStatus>(session.AreaServerInfo);
        }
    }
}
