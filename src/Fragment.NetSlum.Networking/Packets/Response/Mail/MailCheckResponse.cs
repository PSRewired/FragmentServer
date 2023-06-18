using System;
using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Mail;

public class MailCheckResponse : BaseResponse
{
    private bool _hasMail;

    public MailCheckResponse SetHasMail(bool mail)
    {
        _hasMail = mail;

        return this;
    }


    public override FragmentMessage Build()
    {
        var hasMail = _hasMail ? 0x100 : 0x00;

        // Not sure if this is even correct, but basing it from the port
        var unkPrefix = !_hasMail ? 0x01 : 0x00;

        var buffer = new Memory<byte>(new byte[4]);
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Span[..2], (ushort) unkPrefix);
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Span[2..4], (ushort) hasMail);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataMailCheckSuccess,
            Data = buffer,
        };
    }
}
