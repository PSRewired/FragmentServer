using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class AddItemToGuildInventoryResponse : BaseResponse
{
    private readonly ushort _quantityAdded;

    public AddItemToGuildInventoryResponse(ushort quantityAdded)
    {
        _quantityAdded = quantityAdded;
    }

    public override FragmentMessage Build()
    {
        var writer = new MemoryWriter(sizeof(ushort));
        writer.Write(_quantityAdded);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataAddItemToGuildResponse,
            Data = writer.Buffer,
        };
    }
}
