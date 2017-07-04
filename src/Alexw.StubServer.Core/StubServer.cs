using System;
using System.Collections.Generic;
using Alexw.StubServer.Core.Middleware;
using Microsoft.Owin.Hosting;
using Owin;

namespace Alexw.StubServer.Core
{
    public class StubServer : IDisposable
    {
        public List<RecordedRequest> Recorded { get; }
        public string Address { get; set; }

        public Rules Rules;
        private IDisposable _webServer;

        public StubServer()
        {
            Recorded = new List<RecordedRequest>();
            Rules = new Rules();
        }

        public void Start(string address)
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