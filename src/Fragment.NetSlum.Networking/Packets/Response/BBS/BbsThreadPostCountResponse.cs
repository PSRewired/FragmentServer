using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.BBS;

public class BbsThreadPostCountResponse : BaseResponse
{
    private readonly uint _numEntries;

    public BbsThreadPostCountResponse(uint numEntries)
    {
        _numEntries = numEntries;
    }

    public override FragmentMessage Build()
    {
        var buffer = new Memory<byte>(new byte[4]);
        BinaryPrimitives.WriteUInt32BigEndian(buffer.Span, _numEntries);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataBbsThreadPostCountResponse,
            Data = buffer,
        };
    }
}
