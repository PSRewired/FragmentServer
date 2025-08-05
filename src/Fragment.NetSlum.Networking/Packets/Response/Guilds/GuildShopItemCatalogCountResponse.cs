using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class GuildShopItemCatalogCountResponse : BaseResponse
{
    private readonly ushort _count;

    public GuildShopItemCatalogCountResponse(ushort count)
    {
        _count = count;
    }

    public override FragmentMessage Build()
    {
        var writer = new MemoryWriter(2);
        writer.Write(_count);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataGuildShopItemCatalogCountResponse,
            Data = writer.Buffer,
        };
    }
}
