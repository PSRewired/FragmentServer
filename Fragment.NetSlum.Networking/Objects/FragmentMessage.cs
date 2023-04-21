using System.Buffers.Binary;
using System.Text;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Crypto;

namespace Fragment.NetSlum.Networking.Objects;

public class FragmentMessage
{
    public OpCodes OpCode { get; set; }
    public Memory<byte> Data { get; set; }
    public ushort Checksum => BlowfishProvider.Checksum(Data.ToArray());

    public byte[] ToArray()
    {
        var buffer = new byte[Data.Length + 6];
        var span = new Span<byte>(buffer);

        BinaryPrimitives.WriteUInt16BigEndian(span[..2], (ushort)(Data.Length + 6));
        BinaryPrimitives.WriteUInt16BigEndian(span[2..4], (ushort)OpCode);
        BinaryPrimitives.WriteUInt16BigEndian(span[4..6], Checksum);
        Data.Span.CopyTo(span[6..]);

        return buffer;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append($"[OP: {OpCode}({(ushort)OpCode:X4})");
        sb.Append($", CHK: {Checksum:X4}({Checksum})]");
        sb.AppendLine("");
        sb.AppendLine("Data:");
        sb.AppendLine(Data.ToHexDump());

        return sb.ToString();
    }
}
