using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using Fragment.NetSlum.Networking.Constants;
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
            var checksum = response.Checksum;

            /*
            if (response.MessageType == MessageType.Data)
            {
                var paddedBuff = new Memory<byte>(new byte[GetPaddedBufferLength(response.Data.Length)]);
                response.Data.CopyTo(paddedBuff);
                response.Data = paddedBuff;
            }
            */

            // Need to copy the checksum to the payload before encryption
            var payloadLength = GetPaddedBufferLength(response.Data.Length + 2);

            using var bufferOwner = MemoryPool<byte>.Shared.Rent(payloadLength);
            var buffer = bufferOwner.Memory.Span;

            BinaryPrimitives.WriteUInt16BigEndian(buffer[..2],checksum);
            response.Data.Span.CopyTo(buffer[2..]);

            if (_cryptoHandler.TryEncrypt(buffer[..payloadLength].ToArray(), out var encryptedData))
            {
                _logger.LogDebug("Encrypting packet:\n{HexDump}", response.ToString());
                response.Encrypted = true;
                response.Data = encryptedData;
            }
        }
    }
    /// <summary>
    /// Fragment requires data packets to be packed into buffers aligned by 8 bytes
    /// </summary>
    /// <param name="dataLength"></param>
    /// <returns></returns>
    private static int GetPaddedBufferLength(int dataLength)
    {
        return (dataLength + 7) & ~7;
    }
}
