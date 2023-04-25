using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Character;

public class Select2CharacterResponse : BaseResponse
{
    public override FragmentMessage Build()
    {
        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataSelectCharOk,
            Data = new Memory<byte>(new byte[] {0x30, 0x30, 0x30, 0x30}), // Apparently represents "0000" literally
        };
    }
}
