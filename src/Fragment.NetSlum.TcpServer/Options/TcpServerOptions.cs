namespace Fragment.NetSlum.TcpServer.Options;

public class TcpServerOptions
{
    /// <summary>
    /// IP Address the server should attempt to bind
    /// </summary>
    public string IpAddress { get; set; } = "0.0.0.0";

    /// <summary>
    /// The port the server should listen on
    /// </summary>
    public int Port { get; set; } = 49000;

    /// <summary>
    /// The number of seconds in which a session will be considered timed-out
    /// </summary>
    public int SessionTimeout { get; set; } = 60;

    /// <summary>
    /// Manual mode will cause all sessions to block sending data until their respective Flush() methods are called.
    /// Useful for synchronizing clients in a logic loop, or for packet aggregation
    /// </summary>
    public bool ManualMode { get; set; } = false;

    public int ReceiveBufferSize { get; set; } = 8192;
    public int SendBufferSize { get; set; } = 8192;
}
