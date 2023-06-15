using Fragment.NetSlum.TcpServer.Options;

namespace Fragment.NetSlum.Server.Servers;

public class ServerConfiguration : TcpServerOptions
{
    public new bool ManualMode { get; set; } = true;
    public int TickRate { get; set; } = 30;
}
