using System;
using Microsoft.Owin.Hosting;
using Owin;

namespace Alexw.StubServer.Core
{
    public class SelfHostedStubServer : IDisposable
    {
        public Uri Address { get; set; }
        private readonly IDisposable _instance;

        public SelfHostedStubServer() : this("http://localhost:" + TcpPorts.GetFreeTcpPort())
        { 
            // use random port
        }

        public SelfHostedStubServer(string address)
        {
            if (!Uri.IsWellFormedUriString(address, UriKind.Absolute))
                throw new ArgumentException("Address must be well formed");

            Address = new Uri(address);

            _instance = WebApp.Start(
        }


        public void Dispose()
        {
            _instance.Dispose();
        }
    }
}