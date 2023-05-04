using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using System.Buffers.Binary;
using Fragment.NetSlum.Core.Extensions;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby
{
    public class ChatLobbyEntryResponse : BaseResponse
    {
        private ushort _chatLobbyId { get; set; }
        private string _chatLobbyName { get; set; } = "";
        private ushort _clientCount { get; set; }

        public ChatLobbyEntryResponse SetChatLobbyId(ushort id)
        {
            _chatLobbyId = id;
            return this;
        }
        public ChatLobbyEntryResponse SetChatLobbyName(string name)
        {
            _chatLobbyName = name;
            return this;
        }
        public ChatLobbyEntryResponse SetClientCount(ushort count)
        {
            _clientCount = count;
            return this;
        }


        public override FragmentMessage Build()
        {          
            var channelName = _chatLobbyName.ToShiftJis();
            int pos = 0;
            var bufferMemory = new Memory<byte>(new byte[channelName.Length + sizeof(int) + 4]);
            var buffer = bufferMemory.Span;

            BinaryPrimitives.WriteUInt16BigEndian(buffer[..2], _chatLobbyId);
            pos += 2;
            channelName.CopyTo(buffer[2..]);
            pos += channelName.Length + 1;
            BinaryPrimitives.WriteUInt16BigEndian(buffer[pos..], _clientCount);
            pos += 2;
            BinaryPrimitives.WriteUInt16BigEndian(buffer[pos..], (ushort)(_clientCount + 1)); // Hard Coded max client Count?
            return new FragmentMessage
            {
                OpCode = OpCodes.Data,
                DataPacketType = OpCodes.DataLobbyEntryLobby,
                Data = bufferMemory,
            };
        }
    }
}
