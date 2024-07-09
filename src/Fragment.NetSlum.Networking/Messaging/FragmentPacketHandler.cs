using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.DependencyInjection;
using Fragment.NetSlum.Networking.Extensions;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Request;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Messaging;

public class FragmentPacketHandler : IPacketHandler<FragmentMessage>
{
    private readonly ILogger<FragmentPacketHandler> _logger;
    private readonly PacketCache _packetCache;

    public FragmentPacketHandler(ILogger<FragmentPacketHandler> logger, PacketCache packetCache)
    {
        _logger = logger;
        _packetCache = packetCache;
    }

    public async ValueTask<ICollection<FragmentMessage>> CreateResponse<TSession>(TSession session, FragmentMessage o) where TSession : IScopeable
    {
        BaseRequest? availableResponseObject = GetRequest(session.ServiceScope.ServiceProvider, o);

        if (availableResponseObject == null)
        {
            return Array.Empty<FragmentMessage>();
        }

        return await availableResponseObject.CreateResponse(session, o);
    }

    private BaseRequest? GetRequest(IServiceProvider serviceProvider, FragmentMessage o)
    {
        var pType = _packetCache.GetRequest(o);

        // Skip empty objects.
        if (pType == null)
        {
            _logger.LogWarning("No response registered for {Object}", o);
            return null;
        }

        if (serviceProvider.GetService(pType) is not BaseRequest request)
        {
            _logger.LogWarning(
                "A packet containing the [FragmentPacket] annotation was found but was not registered for use. Name:  {Object}",
                pType);

            return null;
        }

//#if DEBUG
        _logger.LogDebug("Response available! [{PacketName}] \n{Object}", request.GetType().Name,
            o);
//#endif
        return request;
    }
}
