using System;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Character;

public class Select2CharacterResponse : BaseResponse
{
    public override FragmentMessage Build()
    {
        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.Data_Select2_CharacterSuccess,
            Data = new Memory<byte>(new byte[] {0x30, 0x30, 0x30, 0x30}), // Apparently represents "0000" literally
        };
    }
}
