using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Crypto;
using Fragment.NetSlum.Networking.Objects;
using Serilog;

namespace Fragment.NetSlum.Networking.Pipeline.Decoders;

public class FragmentFrameDecoder : IPacketDecoder
{
    private readonly CryptoHandler _cryptoHandler;

    public FragmentFrameDecoder(CryptoHandler cryptoHandler)
    {
        _cryptoHandler = cryptoHandler;
    }

    public int Decode(Memory<byte> data, List<FragmentMessage> messages)
    {
        var stream = new MemoryStream(data.ToArray());
        var reader = new BinaryReader(stream);

        int pos = 0;

        // Not enough frame data to read
        if (data.Length < 2)
        {
            return 0;
        }

        ushort datalen = BinaryPrimitives.ReadUInt16BigEndian(data.Span[pos..2]);
        pos += 2;

        // If the length designated by the packet is larger than our incoming data,
        // then we haven't received all of the content
        if (datalen > data.Length - 2)
        {
            return 0;
        }

        if (datalen < 1)
        {
            return pos;
        }

        var messageContent = data.Span[pos..(pos+datalen)];
        pos += messageContent.Length;

        var code = (OpCodes)BinaryPrimitives.ReadUInt16BigEndian(messageContent[..2]);
        messageContent = messageContent[2..];

        if (messageContent.Length < 3)
        {
            return pos;
        }

        var ok = _cryptoHandler.TryDecrypt(messageContent.ToArray(), out var decrypted);

        Log.Information("[CRYPTO] Decrypt Result: {Result}", ok ? "OK" : "FAIL");

        messages.Add(new FragmentMessage
        {
            OpCode = code,
            Data = decrypted,
        });

        return data.Length;
    }
}
