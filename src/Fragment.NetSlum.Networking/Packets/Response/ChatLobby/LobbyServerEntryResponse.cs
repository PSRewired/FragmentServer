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

    public LobbyServerEntryResponse SetStatus(byte status)
    {
        _state = status;

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

        var writer = new MemoryWriter(3 + ipAddressFlipped.Length + nameBytes.Length + _details.Length + sizeof(ushort) * 4);

        writer.Skip(1); // This is likely the disc ID NOP'd out

        writer.Write(ipAddressFlipped);
        writer.Write((ushort)_serverIp.Port);

        writer.Write(_serverName.ToShiftJis());
        //writer.Skip(1); // String null terminator

        writer.Write(_level);
        writer.Write((ushort)_state); // Maybe some sort of online status? The server shows up as "incapacitated" if the value is 0
        writer.Write(_playerCount);

        writer.Write(_details);

        return new FragmentMessage
        {
            OpCode = OpCodes.Data,
            DataPacketType = OpCodes.DataLobbyGetServersEntryServer,
            Data = writer.Buffer,
        };
    }
}
