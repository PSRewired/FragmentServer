using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Guilds;

public class GuildDonationSettingsResponse : BaseResponse
{
    private bool _isGuildMaster = false;

    public GuildDonationSettingsResponse SetIsGuildMaster(bool isGuildMaster)
    {
        _isGuildMaster = isGuildMaster;

        return this;
    }

    public override FragmentMessage Build()
    {
        var writer = new MemoryWriter(sizeof(uint) * 2);

        if (_isGuildMaster)
        {
            writer.Write(1u);
            writer.Write(1u);
        }

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataGuildDonationSettingsResponse,
            Data = writer.Buffer,
        };
    }
}
