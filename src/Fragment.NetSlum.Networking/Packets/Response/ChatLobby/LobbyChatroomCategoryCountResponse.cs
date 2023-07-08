using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby;

public class LobbyChatroomCategoryCountResponse : BaseResponse
{
    private readonly ushort _numLobbies;

    public LobbyChatroomCategoryCountResponse(ushort numLobbies)
    {
        _numLobbies = numLobbies;
    }

    public override FragmentMessage Build()
    {
        var writer = new MemoryWriter(sizeof(ushort)*6);
        writer.Write((ushort)1);
        writer.Write(_numLobbies);
        writer.Write((ushort)1);
        writer.Write((ushort)1);
        writer.Write((ushort)1);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataLobbyChatroomCategory,
            Data = writer.Buffer,
        };
    }
}
