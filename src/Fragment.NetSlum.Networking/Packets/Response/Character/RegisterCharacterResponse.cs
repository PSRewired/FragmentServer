using System.Buffers.Binary;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Character;

public class RegisterCharacterResponse : BaseResponse
{
    private GuildStatus _guildStatus = GuildStatus.None;
    private ushort _guildId;

    public RegisterCharacterResponse SetGuildStatus(GuildStatus status)
    {
        _guildStatus = status;

        return this;
    }

    public RegisterCharacterResponse SetGuildId(ushort gid)
    {
        _guildId = gid;

        return this;
    }

    public override FragmentMessage Build()
    {
        var buffer = new Memory<byte>(new byte[3]);
        var bufferSpan = buffer.Span;

        bufferSpan[0] = (byte)_guildStatus;
        BinaryPrimitives.WriteUInt16BigEndian(bufferSpan[1..], _guildId);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataRegisterCharOk,
            Data = buffer,
        };
    }
}
