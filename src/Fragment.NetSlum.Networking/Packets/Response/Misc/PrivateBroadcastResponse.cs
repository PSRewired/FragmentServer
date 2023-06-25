using System;
using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.Misc;

public class PrivateBroadcastResponse : BaseResponse
{
    private ushort _lobbyId { get; set; }
    private ushort _senderIndex { get; set; }
    private bool _isSender { get; set; }
    private Memory<byte> _data { get; set; } = Array.Empty<byte>();

    public PrivateBroadcastResponse SetLobbyId(ushort id)
    {
        _lobbyId = id;

        return this;
    }

    public PrivateBroadcastResponse SetSenderIndex(ushort idx)
    {
        _senderIndex = idx;

        return this;
    }

    public PrivateBroadcastResponse SetIsSender(bool sender)
    {
        _isSender = sender;

        return this;
    }

    public PrivateBroadcastResponse SetData(Memory<byte> data)
    {
        _data = data;

        return this;
    }

    public override FragmentMessage Build()
    {
        var writer = new MemoryWriter(_data.Length + sizeof(ushort) * 2);

        writer.Write(_lobbyId);
        writer.Write(_isSender ? (ushort)0xFFFF : _senderIndex);
        writer.Write(_data);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.PrivateBroadcast,
            Data = writer.Buffer,
        };
    }
}
