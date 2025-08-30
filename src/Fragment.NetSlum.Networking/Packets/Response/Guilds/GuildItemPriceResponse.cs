using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class GuildItemPriceResponse : BaseResponse
{
    private uint _generalPrice;
    private uint _memberPrice;

    public GuildItemPriceResponse SetGeneralPrice(uint generalPrice)
    {
        _generalPrice = generalPrice;

        return this;
    }

    public GuildItemPriceResponse SetMemberPrice(uint memberPrice)
    {
        _memberPrice = memberPrice;

        return this;
    }

    public override FragmentMessage Build()
    {
        var writer = new MemoryWriter(sizeof(uint) * 2);

        writer.Write(_generalPrice);
        writer.Write(_memberPrice);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataGuildItemPriceResponse,
            Data = writer.Buffer,
        };
    }
}
