using System;
using System.Buffers.Binary;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Saves;

public class PlayerAccountInformationResponse : BaseResponse
{
    private int _accountId;
    private string _motd = "";

    public PlayerAccountInformationResponse SetAccountId(int id)
    {
        _accountId = id;

        return this;
    }

    public PlayerAccountInformationResponse SetMessageOfTheDay(string motd)
    {
        _motd = motd;

        return this;
    }

    public override FragmentMessage Build()
    {
        var motd = _motd.ToShiftJis();

        var bufferMemory = new Memory<byte>(new byte[motd.Length + sizeof(int) + 1]);
        var buffer = bufferMemory.Span;

        BinaryPrimitives.WriteUInt32BigEndian(buffer[..4], (uint)_accountId);
        buffer[4] = (byte)(motd.Length - 1);
        motd.CopyTo(buffer[5..]);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataPlayerAccountInfoSuccess,
            Data = bufferMemory.EnsureSize(0x200),
        };
    }
}
