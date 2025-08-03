using System;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.AreaServer;

public class AreaServerFavoritesNoResultResponse : BaseResponse
{
    public override FragmentMessage Build()
    {
        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataAreaServerFavoriteEntry,
            Data = Array.Empty<byte>(),
        };
    }
}
