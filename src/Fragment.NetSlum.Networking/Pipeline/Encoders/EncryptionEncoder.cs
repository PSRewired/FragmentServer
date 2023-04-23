using System.Buffers;
using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Crypto;
using Fragment.NetSlum.Networking.Objects;
using Microsoft.Extensions.Logging;

namespace Fragment.NetSlum.Networking.Pipeline.Encoders;

public class EncryptionEncoder : IMessageEncoder
{
    private readonly CryptoHandler _cryptoHandler;
    private readonly ILogger _logger;

    public EncryptionEncoder(CryptoHandler cryptoHandler, ILogger<EncryptionEncoder> logger)
    {
        _cryptoHandler = cryptoHandler;
        _logger = logger;
    }

    public void Encode(List<FragmentMessage> responseObjects, MemoryStream memoryStream)
    {
        foreach (var response in responseObjects)
        {
            // Need to copy the checksum to the payload before encryption
            var payloadLength = response.Data.Length + 2;

            using var bufferOwner = MemoryPool<byte>.Shared.Rent(payloadLength);
            var buffer = bufferOwner.Memory.Span;

            if (response.OpCode == Constants.OpCodes.Data)
            {
                BinaryPrimitives.WriteUInt16BigEndian(buffer[..2], response.DataChecksum);
            }
            else
            {
                BinaryPrimitives.WriteUInt16BigEndian(buffer[..2], response.Checksum);
            }

            
            response.Data.Span.CopyTo(buffer[2..]);

            if (_cryptoHandler.TryEncrypt(buffer[..payloadLength].ToArray(), out var encryptedData))
            {
                _logger.LogDebug("Encrypting packet:\n{HexDump}", response.ToString());
                response.Encrypted = true;
                response.Data = encryptedData;
            }
        }
    }
}
