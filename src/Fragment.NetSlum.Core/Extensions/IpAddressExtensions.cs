using System.Net;

namespace Fragment.NetSlum.Core.Extensions;

public static class IpAddressExtensions
{
    ///<summary>
    /// Private IP CIDR ranges defined by
    /// <a href="http://www.faqs.org/rfcs/rfc1918.html">RFC1918</a>
    /// </summary>
    private static readonly string[] _privateIpCidrs = new string[]
    {
        "10.0.0.0/8",
        "172.16.0.0/12",
        "192.168.0.0/16",
    };

    /// <summary>
    /// Determines if the given <see cref="IPAddress"/> is a private (LAN) IP
    /// by comparing it to the CIDR ranges defined in <a href="http://www.faqs.org/rfcs/rfc1918.html">RFC1918</a>
    /// </summary>
    /// <param name="addr"></param>
    /// <returns></returns>
    public static bool IsPrivate(this IPAddress addr)
    {
        foreach (var cidr in _privateIpCidrs)
        {
            var network = IPNetwork.Parse(cidr);

            if (!network.Contains(addr))
            {
                continue;
            }

            return true;
        }

        return false;
    }
}
