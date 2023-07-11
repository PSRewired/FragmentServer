using System;
using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Mail;

public class MailListCountResponse : BaseResponse
{
    private readonly uint _numMailEntries;

    public MailListCountResponse(uint numMailEntries)
    {
        _numMailEntries = numMailEntries;
    }

    public override FragmentMessage Build()
    {
        var buffer = new Memory<byte>(new byte[4]);
        BinaryPrimitives.WriteUInt32BigEndian(buffer.Span, _numMailEntries);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataMailCountResponse,
            Data = buffer,
        };
    }
}
