using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Crypto;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Packets.Request;

[FragmentPacket(OpCodes.KeyExchangeRequest)]
public class KeyExchangeRequest : BaseRequest
{
    private readonly CryptoHandler _cryptoHandler;

    public KeyExchangeRequest(CryptoHandler cryptoHandler)
    {
        _cryptoHandler = cryptoHandler;
    }

    public override async Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        var clientKey = request.Data[4..];
        _cryptoHandler.ClientCipher.PrepareNewKey(clientKey.ToArray());

        var serverCipher = BlowfishProvider.CreateNew(out var serverKey);
        _cryptoHandler.ServerCipher.PrepareNewKey(serverKey);

        Log.Information("Received client key!\n{HexDump}", clientKey.ToHexDump());
        return new[]
        {
            new KeyExchangeResponse()
                .SetClientKey(clientKey)
                .SetServerKey(serverKey)
                .Build(),
        };
    }
}
