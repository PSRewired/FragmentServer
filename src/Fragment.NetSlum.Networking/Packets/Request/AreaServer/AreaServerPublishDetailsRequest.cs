using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Events;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.AreaServer;
using Fragment.NetSlum.Networking.Packets.Response;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.Persistence;
using Fragment.NetSlum.Persistence.Entities;
using Fragment.NetSlum.Server.Stores;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Packets.Request.AreaServer;

[FragmentPacket(MessageType.Data, OpCodes.Data_AreaServerPublishDetails1Request)]
[FragmentPacket(MessageType.Data, OpCodes.Data_AreaServerPublishDetails2Request)]
[FragmentPacket(MessageType.Data, OpCodes.Data_AreaServerPublishDetails3Request)]
[FragmentPacket(MessageType.Data, OpCodes.Data_AreaServerPublishDetails4Request)]
public partial class AreaServerPublishDetailsRequest : BaseRequest
{
    private readonly FragmentContext _database;
    private readonly ILogger<AreaServerPublishDetailsRequest> _logger;
    private readonly ICommandBus _commandBus;
    private readonly AreaServerAssociationStore _associationStore;

    public AreaServerPublishDetailsRequest(FragmentContext database, ILogger<AreaServerPublishDetailsRequest> logger,
        ICommandBus commandBus, AreaServerAssociationStore associationStore)
    {
        _database = database;
        _logger = logger;
        _commandBus = commandBus;
        _associationStore = associationStore;
    }

    public override ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        BaseResponse response;

        switch (request.DataPacketType)
        {
            case OpCodes.Data_AreaServerPublishDetails1Request:
                session.AreaServerInfo!.DiskId = request.Data[..0x40].Span.GetNullTerminatedString();

                var nameBytes = request.Data[0x41..].Span.ReadToNullByte();

                var areaServerName = nameBytes.ToShiftJisString();

                var claimMatches = AreaServerClaimRegex.Match(areaServerName);

                var association = _database.AreaServerAssociations
                    .FirstOrDefault(a => a.PublicIpAddress == session.AreaServerInfo.PublicConnectionEndpoint!.Address.ToString());

                if (association == null && claimMatches.Success && _associationStore.TryClaimCode(claimMatches.Groups[1].Value, out var claimAssociation))
                {
                    association = new AreaServerAssociation
                    {
                        AuthUserId = claimAssociation!.AuthUserId,
                        LocalIpAddress = session.AreaServerInfo.PrivateConnectionEndpoint!.Address.ToString(),
                        PublicIpAddress = session.AreaServerInfo.PublicConnectionEndpoint!.Address.ToString(),
                        LastKnownName = areaServerName,
                    };

                    _database.AreaServerAssociations.Add(association);

                    areaServerName = areaServerName.Replace($"#{claimMatches.Groups[1].Value}", "", StringComparison.OrdinalIgnoreCase);
                }

                association?.LastKnownName = areaServerName;
                _database.SaveChanges();

                session.AreaServerInfo!.ServerName = areaServerName;

                var pos = 0x41 + nameBytes.Length + 1;

                session.AreaServerInfo!.Level = BinaryPrimitives.ReadUInt16BigEndian(request.Data[pos..(pos + 2)].Span);
                pos += 3;
                session.AreaServerInfo!.State = (AreaServerState)request.Data.Span[pos++];
                session.AreaServerInfo!.CurrentPlayerCount = BinaryPrimitives.ReadUInt16BigEndian(request.Data[pos..(pos + 2)].Span);
                pos += 3;
                session.AreaServerInfo.ServerId = request.Data[pos..];

                _logger.LogInformation("Area Server Published Details: {NewLine}{AreaServerInfo}", Environment.NewLine,
                    session.AreaServerInfo!.ToString());

                _commandBus.Notify(new AreaServerPublishedEvent(session.AreaServerInfo!)).Wait();

                response = new AreaServerPublishDetailsResponse
                {
                    PacketType = OpCodes.Data_AreaServerPublishDetails1Success, Data =
                    [
                        0x00, 0x01
                    ]
                };
                break;
            case OpCodes.Data_AreaServerPublishDetails2Request:
                response = new AreaServerPublishDetailsResponse
                {
                    PacketType = OpCodes.Data_AreaServerPublishDetails2Success, Data =
                    [
                        0xDE, 0xAD
                    ]
                };
                break;

            case OpCodes.Data_AreaServerPublishDetails3Request:
                response = new AreaServerPublishDetailsResponse
                {
                    PacketType = OpCodes.Data_AreaServerPublishDetails3Success, Data =
                    [
                        0x00, 0x01
                    ]
                };
                break;
            case OpCodes.Data_AreaServerPublishDetails4Request:
                response = new AreaServerPublishDetailsResponse
                {
                    PacketType = OpCodes.Data_AreaServerPublishDetails4Success, Data =
                    [
                        0x00, 0x01
                    ]
                };
                break;
            default:
                return NoResponse();
        }

        return SingleMessage(response.Build());
    }

    [GeneratedRegex("#([\\d]{6})", RegexOptions.Compiled, 1000)]
    private partial Regex AreaServerClaimRegex { get; }
}
