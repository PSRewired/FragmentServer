using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;


namespace Fragment.NetSlum.Networking.Packets.Response.AreaServer;

public class AreaServerPublishDetailsResponse :BaseResponse
{
    public OpCodes PacketType { get; set; }
    public byte[] Data { get; set; } = [];

    public override FragmentMessage Build()
    {
        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = PacketType,
            Data = Data,
        };
    }
}
