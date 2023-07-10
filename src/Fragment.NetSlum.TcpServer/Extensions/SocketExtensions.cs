using System.Net.Sockets;

namespace Fragment.NetSlum.TcpServer.Extensions;

public static class SocketExtensions
{
    /// <summary>
    /// Returns the IP Address of the connected client as a string
    /// </summary>
    /// <param name="socket"></param>
    /// <remarks>If using a reverse proxy, this will return the address of the proxy instead</remarks>
    /// <returns></returns>
    public static string GetClientIp(this Socket socket)
    {
        return socket.RemoteEndPoint?.ToString() == null ? string.Empty : socket.RemoteEndPoint.ToString()!.Split(":")[0];
    }

    /// <summary>
    /// Returns the IP address of the server.
    /// <remarks>This will return the IP address that the server is bound to by the OS. This could contain a LAN IP if using v/LAN</remarks>
    /// </summary>
    /// <param name="socket"></param>
    /// <returns></returns>
    public static string GetServerIp(this Socket socket)
    {
        return socket.LocalEndPoint?.ToString() == null ? string.Empty : socket.LocalEndPoint.ToString()!.Split(":")[0];
    }
}
