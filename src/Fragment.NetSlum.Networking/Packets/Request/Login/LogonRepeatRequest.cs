using Fragment.NetSlum.Networking.Attributes;
using Fragment.NetSlum.Networking.Constants;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Packets.Response.Login;
using Fragment.NetSlum.Networking.Packets.Response;
using Fragment.NetSlum.Networking.Sessions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fragment.NetSlum.Networking.Packets.Request.Login
{
    [FragmentPacket(OpCodes.Data, OpCodes.Data_LogonRepeatRequest)]
    public class LogonRepeatRequest :BaseRequest
    {
        private readonly ILogger<LogonRequest> _logger;

        public LogonRepeatRequest(ILogger<LogonRequest> logger)
        {
            _logger = logger;
        }

        public override Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request)
        {
            BaseResponse response = new LogonRepeatResponse();

            return Task.FromResult<ICollection<FragmentMessage>>(new[] { response.Build() });
        }
    }
}
