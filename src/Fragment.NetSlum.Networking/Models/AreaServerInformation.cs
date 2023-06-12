using System;
using System.Net;
using System.Text;
using Fragment.NetSlum.Core.Extensions;

namespace Fragment.NetSlum.Networking.Models;

public class AreaServerInformation
{
    //Area Server Fields
    public string DiskId { get; set; } = "";
    public string ServerName { get; set; } = "";
    public ushort Level { get; set; }
    public byte State { get; set; }
    public ushort CurrentPlayerCount { get;set; }
    public Memory<byte> Detail { get; set; } = Array.Empty<byte>();
    public IPEndPoint? ConnectionEndpoint { get; set; }

    public override string ToString()
    {
        var sb = new StringBuilder("--- Area Server Information ---\n");
        sb.AppendLine($"Server Name: {ServerName}");
        sb.AppendLine($"Current Level: {Level}");
        sb.AppendLine($"Current Status: {State}(0x{State:X1})");
        sb.AppendLine($"Current Player Count: {CurrentPlayerCount}");
        sb.AppendLine($"Connection Info: {ConnectionEndpoint?.ToString()}");
        sb.AppendLine($"Known Detail: \n{Detail.ToHexDump()}\n");

        return sb.ToString();
    }
}
