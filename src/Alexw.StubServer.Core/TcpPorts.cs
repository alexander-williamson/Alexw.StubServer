using System.Net;
using System.Net.Sockets;

namespace Alexw.StubServer.Core
{
    // modified from https://stackoverflow.com/questions/138043/find-the-next-tcp-port-in-net
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
