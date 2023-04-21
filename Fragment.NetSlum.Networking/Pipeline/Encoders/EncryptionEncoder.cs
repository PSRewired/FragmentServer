using Fragment.NetSlum.Networking.Crypto;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Pipeline.Encoders;

public class EncryptionEncoder : IMessageEncoder
{
    private readonly CryptoHandler _cryptoHandler;

    public EncryptionEncoder(CryptoHandler cryptoHandler)
    {
        _cryptoHandler = cryptoHandler;
    }

    public void Encode(List<FragmentMessage> responseObjects, MemoryStream memoryStream)
    {
        foreach (var response in responseObjects)
        {
            if (_cryptoHandler.TryEncrypt(response.Data.ToArray(), out var encryptedData))
            {
                response.Data = encryptedData;
            }
        }
    }
}
