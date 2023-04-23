using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Crypto;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Sessions;

namespace Fragment.NetSlum.Networking.Packets.Request.Security;

[FragmentPacket(OpCodes.KeyExchangeAcknowledgment)]
public class KeyExchangeAcknowledgementRequest : BaseRequest
{
    private readonly CryptoHandler _cryptoHandler;

    public KeyExchangeAcknowledgementRequest(CryptoHandler cryptoHandler)
    {
        _cryptoHandler = cryptoHandler;
    }

    public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
    {
        _cryptoHandler.ClientCipher.Initialize();
        _cryptoHandler.ServerCipher.Initialize();

        return Task.FromResult<ICollection<FragmentMessage>>(Array.Empty<FragmentMessage>());
    }
}
