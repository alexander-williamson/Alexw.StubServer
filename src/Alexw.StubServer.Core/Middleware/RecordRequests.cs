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
        private readonly ConcurrentStack<RecordedRequest> _recordedRequests;

        public RecordRequests(OwinMiddleware next, ConcurrentStack<RecordedRequest> recordedRequests) : base(next)
        {
            _recordedRequests = recordedRequests;
        }

        public override async Task Invoke(IOwinContext context)
        {
            while (_recordedRequests.Count >= Limit)
            {
                _recordedRequests.TryPop(out RecordedRequest _);
            }

            using (var ms = new MemoryStream())
            {
                context.Request.Body.CopyTo(ms);
                _recordedRequests.Push(new RecordedRequest
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
