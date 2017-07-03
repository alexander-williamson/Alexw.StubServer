using System.Net;
using System.Net.Sockets;

namespace Alexw.StubServer.Core
{
    public static class TcpPorts
    {
        public static int GetFreeTcpPort()
        {
            var l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            var port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }
}