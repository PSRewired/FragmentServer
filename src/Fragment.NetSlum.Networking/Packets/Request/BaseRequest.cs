using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fragment.NetSlum.Networking.Objects;
using Fragment.NetSlum.Networking.Sessions;
using Serilog;

namespace Fragment.NetSlum.Networking.Packets.Request;

public abstract class BaseRequest
{
    public ILogger Log => Serilog.Log.ForContext(GetType());

    public abstract ValueTask<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request);

    /// <summary>
    /// Helper method to return a single message from a request handler
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    protected static ValueTask<ICollection<FragmentMessage>> SingleMessage(FragmentMessage response) =>
        ValueTask.FromResult(SingleMessageAsync(response));

    protected static ICollection<FragmentMessage> SingleMessageAsync(FragmentMessage response) =>
        new[] { response };

    protected static ValueTask<ICollection<FragmentMessage>> NoResponse() =>
        ValueTask.FromResult<ICollection<FragmentMessage>>(Array.Empty<FragmentMessage>());
}
