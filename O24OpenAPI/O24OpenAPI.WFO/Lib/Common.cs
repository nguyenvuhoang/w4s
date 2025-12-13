using System.Net;
using System.Net.Sockets;

namespace O24OpenAPI.WFO.Lib;

public class Common
{
    public static string GetLocalIPAddress()
    {
        IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress[] addressList = hostEntry.AddressList;
        foreach (IPAddress iPAddress in addressList)
        {
            if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
            {
                return iPAddress.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system.");
    }

    public static long GetCurrentDateAsLongNumber()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}
