using System;
using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class GuildMenuCategoryCountResponse : BaseResponse
{
    private readonly ushort _numCategories;

    public GuildMenuCategoryCountResponse(ushort numCategories)
    {
        _numCategories = numCategories;
    }

    public override FragmentMessage Build()
    {
        var buffer = new Memory<byte>(new byte[2]);
        BinaryPrimitives.WriteUInt16BigEndian(buffer.Span, _numCategories);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.Data_GuildCategoryEntryCountResponse,
            Data = buffer,
        };
    }
}
