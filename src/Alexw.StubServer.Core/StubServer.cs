using System.Collections.Generic;
using Alexw.StubServer.Core.Middleware;
using Owin;

namespace Alexw.StubServer.Core
{
    public class StubServer
    {
        public List<RecordedRequest> Recorded { get; }
        public Rules Rules;

        public StubServer()
        {
            Recorded = new List<RecordedRequest>();
            Rules = new Rules();
        }

        public void Configuration(IAppBuilder app)
        {
            app.UseErrorPage();
            app.Use<RecordRequests>();
            app.Use<RespondWithFirstMatch>(Rules);
            app.Use<RespondWithNotFound>();
        }
    }
}