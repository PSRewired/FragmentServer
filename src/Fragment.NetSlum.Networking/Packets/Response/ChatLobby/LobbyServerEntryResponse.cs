using System;
using System.Net;
using Fragment.NetSlum.Core.Buffers;
using Fragment.NetSlum.Core.Extensions;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby;

public class LobbyServerEntryResponse : BaseResponse
{
    private ushort _serverId;
    private IPEndPoint _serverIp = null!;
    private string _serverName = "";
    private byte _state;
    private byte _status;
    private ushort _level;
    private ushort _playerCount;
    private Memory<byte> _details;

    public LobbyServerEntryResponse SetServerId(ushort id)
    {
        _serverId = id;

        return this;
    }

    public LobbyServerEntryResponse SetExternalAddress(IPEndPoint ip)
    {
        _serverIp = ip;

        return this;
    }

    public LobbyServerEntryResponse SetServerName(string name)
    {
        _serverName = name;

        return this;
    }

    public LobbyServerEntryResponse SetState(byte status)
    {
        _state = status;

        return this;
    }

    public LobbyServerEntryResponse SetStatus(byte status)
    {
        _status = status;

        return this;
    }

    public LobbyServerEntryResponse SetLevel(ushort level)
    {
        _level = level;

        return this;
    }

    public LobbyServerEntryResponse SetPlayerCount(ushort count)
    {
        _playerCount = count;

        return this;
    }

    public LobbyServerEntryResponse SetDetails(Memory<byte> details)
    {
        _details = details;

        return this;
    }

    public override FragmentMessage Build()
    {
        var nameBytes = _serverName.ToShiftJis();

        var ipAddressFlipped = new Memory<byte>(new byte[4]);
        _serverIp.Address.TryWriteBytes(ipAddressFlipped.Span, out _);
        ipAddressFlipped.Span.Reverse();

        var writer = new MemoryWriter(ipAddressFlipped.Length + nameBytes.Length + sizeof(ushort) * 4 + 11);
        writer.Skip(1);

        writer.Write(ipAddressFlipped);
        writer.Write((ushort)_serverIp.Port);

        writer.Write(nameBytes);

        writer.Write(_level); // Level
        writer.Write((ushort)_status);
        writer.Write(_state); // State 0 - Normal, 1 - Password ON, 2 - Playing, 3 - Playing, 4 - Incapacitated
        writer.Write(_playerCount);

        // Details appear to be written at 15 or 16
        writer.Write(_details);

        return new FragmentMessage
        {
            MessageType = MessageType.Data,
            DataPacketType = OpCodes.DataLobbyGetServersEntryServer,
            Data = writer.Buffer,
        };
    }
}
