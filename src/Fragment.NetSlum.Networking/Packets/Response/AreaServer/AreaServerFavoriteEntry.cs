using System;
using System.Net;
using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.AreaServer;

public class AreaServerFavoriteEntry : BaseResponse
{
    private ushort _userNum;
    private Memory<byte> _detail;
    private IPEndPoint _location { get; set; } = default!;

    public AreaServerFavoriteEntry SetUserNum(ushort userNum)
    {
        _userNum = userNum;

        return this;
    }

    public AreaServerFavoriteEntry SetDetail(Memory<byte> detail)
    {
        _detail = detail;

        return this;
    }

    public AreaServerFavoriteEntry SetLocation(IPEndPoint location)
    {
        _location = location;

        return this;
    }

    public override FragmentMessage Build()
    {
        var ipAddressFlipped = new Memory<byte>(new byte[4]);
        _location.Address.TryWriteBytes(ipAddressFlipped.Span, out _);
        ipAddressFlipped.Span.Reverse();

        var writer = new MemoryWriter(1 + ipAddressFlipped.Length + sizeof(ushort) + sizeof(uint) + sizeof(ushort) + _detail.Length);
        writer.Skip(1);

        writer.Write(ipAddressFlipped);
        writer.Write((ushort)_location.Port);

        writer.Write(_userNum);

        writer.Write(_detail);

        writer.Write(_userNum);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataAreaServerFavoriteEntry,
            Data = writer.Buffer,
        };
    }
}
