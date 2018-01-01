using System;
using System.Collections.Generic;
using Alexw.StubServer.Core.Middleware;
using Microsoft.Owin.Hosting;
using Owin;

namespace Alexw.StubServer.Core
{
    public class StubServer : IDisposable
    {
        public List<RecordedRequest> Recorded { get; } = new List<RecordedRequest>();
        public string Address { get; set; }

        public Rules Rules = new Rules();
        private IDisposable _webServer;

        public StubServer()
        {
            Start("http://localhost:" + TcpPorts.GetFreeTcpPort());
        }

        public StubServer(string address)
        {
            if (!Uri.IsWellFormedUriString(address, UriKind.Absolute))
            {
                throw new ArgumentException("Not a valid absolute Uri", nameof(address));
            }
            Start(address);
        }

        private void Start(string address)
        {
            Address = address;
            _webServer = WebApp.Start(new StartOptions(address), Configuration);
        }

        public void Configuration(IAppBuilder app)
        {
            app.UseErrorPage();
            app.Use<RecordRequests>(Recorded);
            app.Use<RespondWithFirstMatch>(Rules);
            app.Use<RespondWithNotFound>();
        }

        public void Dispose()
        {
            _webServer?.Dispose();
        }
    }
}