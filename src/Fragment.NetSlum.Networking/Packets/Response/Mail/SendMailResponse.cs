using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using System;
using OpCodes = Fragment.NetSlum.Networking.Constants.OpCodes;

namespace Fragment.NetSlum.Networking.Packets.Response.Mail
{
    public class SendMailResponse :BaseResponse
    {
        private OpCodes _responseCode;

        public SendMailResponse SetStatusCode(OpCodes responseCode)
        {
            _responseCode = responseCode;
            return this;
        }
        public override FragmentMessage Build()
        {
            return new FragmentMessage
            {
                MessageType = MessageType.Data,
                DataPacketType = _responseCode,
                Data = new Memory<byte>(new byte[2]),
            };
        }
    }
}
