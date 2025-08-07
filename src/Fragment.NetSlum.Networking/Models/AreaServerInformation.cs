using System;
using System.Net;
using System.Text;
using Fragment.NetSlum.Core.Constants;
using Fragment.NetSlum.Core.Extensions;

namespace Fragment.NetSlum.Networking.Models;

public class AreaServerInformation
{
    //Area Server Fields
    public string DiskId { get; set; } = "";
    public int CategoryId { get; set; }
    public string ServerName { get; set; } = "";
    public ushort Level { get; set; }
    public AreaServerStatus Status { get; set; }
    public AreaServerState State { get; set; }
    public ushort CurrentPlayerCount { get;set; }
    public Memory<byte> ServerId { get; set; } = Array.Empty<byte>();
    public IPEndPoint? PublicConnectionEndpoint { get; set; }
    public IPEndPoint? PrivateConnectionEndpoint { get; set; }
    public DateTime ActiveSince { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder("--- Area Server Information ---\n");
        sb.AppendLine($"Server Name: {ServerName}");
        sb.AppendLine($"Current Level: {Level}");
        sb.AppendLine($"Current Status: {Status}(0x{(byte)Status:X1})");
        sb.AppendLine($"Current State: {State}(0x{(byte)State:X1})");
        sb.AppendLine($"Current Player Count: {CurrentPlayerCount}");
        sb.AppendLine($"Public connection Info: {PublicConnectionEndpoint?.ToString()}");
        sb.AppendLine($"Private connection Info: {PrivateConnectionEndpoint?.ToString()}");
        sb.AppendLine($"Server ID: \n{ServerId.ToHexDump()}\n");

        return sb.ToString();
    }
}
