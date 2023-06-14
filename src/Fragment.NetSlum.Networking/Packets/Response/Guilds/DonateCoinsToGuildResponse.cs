using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class DonateCoinsToGuildResponse : BaseResponse
{
    private ushort _goldDonated;
    private ushort _silverDonated;
    private ushort _bronzeDonated;

    public DonateCoinsToGuildResponse(ushort gold = 0, ushort silver = 0, ushort bronze = 0)
    {
        _goldDonated = gold;
        _silverDonated = silver;
        _bronzeDonated = bronze;
    }

    public override FragmentMessage Build()
    {
        var writer = new MemoryWriter(sizeof(ushort) * 3);
        writer.Write(_goldDonated);
        writer.Write(_silverDonated);
        writer.Write(_bronzeDonated);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataDonateCoinsToGuildResponse,
            Data = writer.Buffer,
        };
    }
}
