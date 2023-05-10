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
    public class ChatLobbyStatusUpdateResponse:BaseResponse
    {
        private byte[] _lastStatus;
        public ChatLobbyStatusUpdateResponse SetLastStatus(byte[] status)
        {
            _lastStatus = status;
            return this;
        }


        public override FragmentMessage Build()
        {
            return new FragmentMessage
            {
                OpCode = OpCodes.Data,
                DataPacketType = OpCodes.DataLobbyStatusUpdate,
                Data = _lastStatus,
            };
        }
    }
}
