using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Alexw.StubServer.Core.Middleware
{
    public class RecordRequests : OwinMiddleware
    {
        private readonly IList<RecordedRequest> _recordedRequests;

        public RecordRequests(OwinMiddleware next, IList<RecordedRequest> recordedRequests) : base(next)
        {
            _recordedRequests = recordedRequests;
        }

        public override async Task Invoke(IOwinContext context)
        {
            using (var ms = new MemoryStream())
            {
                context.Request.Body.CopyTo(ms);
                _recordedRequests.Add(new RecordedRequest
                {
                    Headers = context.Request.Headers.ToDictionary(k => k.Key, v => v.Value),
                    Body = Encoding.UTF8.GetString(ms.ToArray()),
                    ContentType = context.Request.ContentType,
                    Scheme = context.Request.Scheme,
                    RemoteIpAddress = context.Request.RemoteIpAddress,
                    RemotePort = context.Request.RemotePort
                });
            }

            await Next.Invoke(context);
        }
    }
}
