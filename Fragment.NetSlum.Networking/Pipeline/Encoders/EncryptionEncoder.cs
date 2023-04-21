using System.Buffers;
using System.Buffers.Binary;
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
            // Need to copy the checksum to the payload before encryption
            var payloadLength = response.Data.Length + 2;

            using var bufferOwner = MemoryPool<byte>.Shared.Rent(payloadLength);
            var buffer = bufferOwner.Memory.Span;

            BinaryPrimitives.WriteUInt16BigEndian(buffer[..2], response.Checksum);
            response.Data.Span.CopyTo(buffer[2..]);

            if (_cryptoHandler.TryEncrypt(buffer[..payloadLength].ToArray(), out var encryptedData))
            {
                response.Encrypted = true;
                response.Data = encryptedData;
            }
        }
    }
}
