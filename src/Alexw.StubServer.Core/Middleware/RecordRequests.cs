using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Alexw.StubServer.Core.Middleware
{
    public class RecordRequests : OwinMiddleware
    {
        private const int Limit = 250;
        private readonly ConcurrentQueue<RecordedRequest> _recordedRequests;

        public RecordRequests(OwinMiddleware next, ConcurrentQueue<RecordedRequest> recordedRequests) : base(next)
        {
            _recordedRequests = recordedRequests;
        }

        public override async Task Invoke(IOwinContext context)
        {
            while (_recordedRequests.Count >= Limit)
            {
                _recordedRequests.TryDequeue(out RecordedRequest _);
            }

            using (var ms = new MemoryStream())
            {
                context.Request.Body.CopyTo(ms);
                _recordedRequests.Enqueue(new RecordedRequest
                {
                    Headers = context.Request.Headers.ToDictionary(k => k.Key, v => v.Value),
                    Body = Encoding.UTF8.GetString(ms.ToArray()),
                    ContentType = context.Request.ContentType,
                    Scheme = context.Request.Scheme,
                    RemoteIpAddress = context.Request.RemoteIpAddress,
                    RemotePort = context.Request.RemotePort,
                    Uri = context.Request.Uri
                });
            }

            await Next.Invoke(context);
        }
    }
}
