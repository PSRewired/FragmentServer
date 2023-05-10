using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fragment.NetSlum.Networking.Packets.Response.ChatLobby
{
    public class LobbyEventResponse:BaseResponse
    {
        private byte[] _data;
        private ushort _senderIndex;
        private bool _isSender;
        public LobbyEventResponse SetData(byte[] status)
        {
            _data = status;
            return this;
        }

        public LobbyEventResponse SetSenderIndex(ushort index)
        {
            _senderIndex = index;
            return this;
        }
        public LobbyEventResponse SetIsSender(bool isSender)
        {
            _isSender = isSender;
            return this;
        }


        public override FragmentMessage Build()
        {
            var bufferMemory = new Memory<byte>(new byte[_data.Length]);
            var buffer = bufferMemory.Span;

            _data.CopyTo(buffer[0..]);

            if (!_isSender)
            {
                BinaryPrimitives.WriteUInt16BigEndian(buffer[0..2], _senderIndex);
            }
            else
            {
                BinaryPrimitives.WriteUInt16BigEndian(buffer[0..2], 0xFFFF);
            }

            

            return new FragmentMessage
            {
                OpCode = OpCodes.Data,
                DataPacketType = OpCodes.DataLobbyEvent,
                Data = buffer.ToArray(),
            };
        }
    }
}
