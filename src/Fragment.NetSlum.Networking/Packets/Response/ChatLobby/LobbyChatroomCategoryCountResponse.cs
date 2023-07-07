using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby;

public class LobbyChatroomCategoryCountResponse : BaseResponse
{
    private readonly ushort _id;

    public LobbyChatroomCategoryCountResponse(ushort id)
    {
        _id = id;
    }

    public override FragmentMessage Build()
    {
        var writer = new MemoryWriter(sizeof(ushort));
        writer.Write(_id);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataLobbyChatroomCategory,
            Data = writer.Buffer,
        };
    }
}
