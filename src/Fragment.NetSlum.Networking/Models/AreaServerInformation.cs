using System;
using System.Net;
using System.Text;
using Fragment.NetSlum.Core.Extensions;

namespace Fragment.NetSlum.Networking.Models;

public class AreaServerInformation
{
    //Area Server Fields
    public string DiskId { get; set; } = "";
    public int CategoryId { get; set; }
    public string ServerName { get; set; } = "";
    public ushort Level { get; set; }
    public byte State { get; set; }
    public ushort CurrentPlayerCount { get;set; }
    public Memory<byte> Detail { get; set; } = Array.Empty<byte>();
    public IPEndPoint? PublicConnectionEndpoint { get; set; }
    public IPEndPoint? PrivateConnectionEndpoint { get; set; }
    public DateTime ActiveSince { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder("--- Area Server Information ---\n");
        sb.AppendLine($"Server Name: {ServerName}");
        sb.AppendLine($"Current Level: {Level}");
        sb.AppendLine($"Current Status: {State}(0x{State:X1})");
        sb.AppendLine($"Current Player Count: {CurrentPlayerCount}");
        sb.AppendLine($"Public connection Info: {PublicConnectionEndpoint?.ToString()}");
        sb.AppendLine($"Private connection Info: {PrivateConnectionEndpoint?.ToString()}");
        sb.AppendLine($"Known Detail: \n{Detail.ToHexDump()}\n");

        return sb.ToString();
    }
}
