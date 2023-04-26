using System.Buffers.Binary;
using System.Text;
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
      
        var bufferMemory = new Memory<byte>(new byte[motd.Length + (sizeof(int) * 2)]);
        var buffer = bufferMemory.Span;

        BinaryPrimitives.WriteUInt32LittleEndian(buffer[..4], (uint)_accountId);
        buffer[3] = (byte)(motd.Length - 1);
        //BinaryPrimitives.WriteInt32BigEndian(buffer[..4], motd.Length - 1);
        motd.CopyTo(buffer[4..(4 + motd.Length)]);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.Data_PlayerAccountInfoOk,
            Data = bufferMemory.EnsureSize(0x200),
        };
    }
}
