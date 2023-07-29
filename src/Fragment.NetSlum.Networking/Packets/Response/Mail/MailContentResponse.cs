using System;
using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Mail;

public class MailContentResponse : BaseResponse
{
    private string _content = "";
    private string _avatarDescriptor = "";

    public MailContentResponse SetContent(string content)
    {
        _content = content;

        return this;
    }

    public MailContentResponse SetAvatarDescriptor(string desc)
    {
        _avatarDescriptor = desc;

        return this;
    }

    public override FragmentMessage Build()
    {
        var contentBytes = new Span<byte>(new byte[1200]);
        var avatarBytes = new Span<byte>(new byte[130]);

        _content.ToShiftJis(false).CopyTo(contentBytes);
        _avatarDescriptor.ToShiftJis(false).CopyTo(avatarBytes);

        var writer = new MemoryWriter(contentBytes.Length +
                                      avatarBytes.Length +
                                      sizeof(ushort) * 2 +
                                      sizeof(uint)
        );

        writer.Write((byte)5);
        writer.Skip(3);
        writer.Write((ushort)0);
        writer.Write(new byte[1200].AsSpan());
        writer.Write((ushort)0);
        writer.Write(new byte[130].AsSpan());

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataGetMailContentResponse,
            Data = new byte[2],
        };
    }
}
