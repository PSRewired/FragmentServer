using System;
using System.Net;
using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.AreaServer;

public class AreaServerFavoriteEntry : BaseResponse
{
    private IPEndPoint _serverIp = null!;
    private string _serverName = "";
    private byte _state;
    private ushort _level;
    private ushort _playerCount;
    private Memory<byte> _details;

    public AreaServerFavoriteEntry SetExternalAddress(IPEndPoint ip)
    {
        _serverIp = ip;

        return this;
    }

    public AreaServerFavoriteEntry SetServerName(string name)
    {
        _serverName = name;

        return this;
    }

    public AreaServerFavoriteEntry SetStatus(byte status)
    {
        _state = status;

        return this;
    }

    public AreaServerFavoriteEntry SetLevel(ushort level)
    {
        _level = level;

        return this;
    }

    public AreaServerFavoriteEntry SetPlayerCount(ushort count)
    {
        _playerCount = count;

        return this;
    }

    public AreaServerFavoriteEntry SetDetails(Memory<byte> details)
    {
        _details = details;

        return this;
    }

    public override FragmentMessage Build()
    {
        var ipAddressFlipped = new Memory<byte>(new byte[4]);
        _serverIp.Address.TryWriteBytes(ipAddressFlipped.Span, out _);
        ipAddressFlipped.Span.Reverse();

        var serverNameBytes = _serverName.ToShiftJis();

        var writer = new MemoryWriter(ipAddressFlipped.Length + serverNameBytes.Length + sizeof(ushort) * 4 + 9);

        writer.Write(ipAddressFlipped);
        writer.Write((ushort)_serverIp.Port);

        writer.Write(serverNameBytes);

        writer.Write(_level); // Level
        writer.Write((ushort)1); // ?? -- Must be non-zero for the state to show correctly
        writer.Write((byte)0); // State 0 - Normal, 1 - Password ON, 2 - Playing, 3 - Playing, 4 - Incapacitated -- TODO Fix me
        writer.Write(_playerCount);

        // Details appear to be written at 15 or 16
        writer.Write(_details[1..9]);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataAreaServerFavoriteEntry,
            Data = writer.Buffer,
        };
    }
}
