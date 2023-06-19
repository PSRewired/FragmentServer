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

    public abstract Task<ICollection<FragmentMessage>> GetResponse(FragmentTcpSession session, FragmentMessage request);

    /// <summary>
    /// Helper method to return a single message from a request handler
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    protected static Task<ICollection<FragmentMessage>> ReturnSingle(FragmentMessage response) =>
        Task.FromResult(ReturnSingleAsync(response));

    protected static ICollection<FragmentMessage> ReturnSingleAsync(FragmentMessage response) =>
        new[] { response };

    protected static Task<ICollection<FragmentMessage>> NoResult() =>
        Task.FromResult<ICollection<FragmentMessage>>(Array.Empty<FragmentMessage>());
}
