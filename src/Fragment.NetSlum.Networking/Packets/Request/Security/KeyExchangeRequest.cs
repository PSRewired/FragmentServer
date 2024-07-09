using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Fragment.NetSlum.Core.CommandBus;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Crypto;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Security;
using Fragment.NetSlum.Networking.Queries.Infrastructure;
using Fragment.NetSlum.Networking.Sessions;
using Fragment.NetSlum.TcpServer.Extensions;

namespace Fragment.NetSlum.Networking.Packets.Request.Security;

[FragmentPacket(MessageType.KeyExchangeRequest)]
public class KeyExchangeRequest : BaseRequest
{
    private readonly CryptoHandler _cryptoHandler;
    private readonly ICommandBus _commandBus;

    public KeyExchangeRequest(CryptoHandler cryptoHandler, ICommandBus commandBus)
    {
        _cryptoHandler = cryptoHandler;
        _commandBus = commandBus;
    }

    public override async ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var isClientBanned = await _commandBus.GetResult(new IsIpAddressBannedQuery(session.Socket!.GetClientIp()));

        if (isClientBanned)
        {
            throw new UnauthorizedAccessException(
                $"A client with a banned IP address of {session.Socket!.GetClientIp()} attempted to connect and was disconnected");
        }

        var keySize = BinaryPrimitives.ReadUInt16BigEndian(request.Data.Span[..2]);

        if (keySize != 16)
        {
            throw new DataException($"Invalid key length for key exchange. Expected 16 got {keySize}");
        }

        var clientKey = request.Data[2..(keySize+2)];
        _cryptoHandler.ClientCipher.PrepareNewKey(clientKey.ToArray());

        var serverCipher = BlowfishProvider.CreateNew(out var serverKey);
        _cryptoHandler.ServerCipher.PrepareNewKey(serverKey);

        Log.Information("Received client key!\n{HexDump}", clientKey.ToHexDump());

        return SingleMessageAsync(
            new KeyExchangeResponse()
                .SetClientKey(clientKey)
                .SetServerKey(serverKey)
                .Build());
    }
}
