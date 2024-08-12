using System;
using System.Buffers.Binary;
using System.Text;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Crypto;

namespace Fragment.NetSlum.Networking.Objects;

public class FragmentMessage
{
    public required MessageType MessageType { get; set; }
    public OpCodes DataPacketType { get; set; } = OpCodes.None;
    public Memory<byte> Data { get; set; } = Array.Empty<byte>();
    public ushort Checksum => BlowfishProvider.Checksum(Data.ToArray());
    public bool Encrypted { get; set; }

    /// <summary>
    /// Represents the final size of the packet when serialized.
    /// [DataLength][OpCode][Checksum][UnencryptedPayload]
    /// [DataLength][OpCode][EncryptedData(checksum included)]
    /// </summary>
    public ushort Length => (ushort)(sizeof(OpCodes) + /*(Encrypted ? 0 : sizeof(ushort)) +*/ Data.Length);

    public byte[] ToArray()
    {
        var length = sizeof(ushort) + Length;
        var buffer = length <= 8192 ? stackalloc byte[length] : new byte[length];
        const int dataOffset = 4;

        BinaryPrimitives.WriteUInt16BigEndian(buffer[..2], Length);
        BinaryPrimitives.WriteUInt16BigEndian(buffer[2..4], (ushort)MessageType);

        // If the data of this message is already encrypted, it is assumed that the checksum was already appended since it needs to be
        // included in the encrypted payload
        if (!Encrypted)
        {
            //BinaryPrimitives.WriteUInt16BigEndian(span[4..6], Checksum);
            //dataOffset += 2;
        }

        Data.Span.CopyTo(buffer[dataOffset..]);

        return buffer.ToArray();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append($"[OP: {MessageType}({(ushort)MessageType:X4})");
        if (MessageType == MessageType.Data)
        {
            sb.Append($", DTYPE: {DataPacketType}({(ushort)DataPacketType:X4})");
        }

        sb.Append($", CHK: {Checksum:X4}({Checksum})]");
        sb.AppendLine("");
        sb.AppendLine("Data:");
        sb.AppendLine(Data.ToHexDump());
        sb.AppendLine($"({ToArray().ToHexString()})");

        return sb.ToString();
    }
}
