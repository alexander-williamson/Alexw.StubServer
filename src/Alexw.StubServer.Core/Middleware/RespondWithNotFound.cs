using System.Threading.Tasks;
using Microsoft.Owin;

namespace Alexw.StubServer.Core.Middleware
{
    public class RespondWithNotFound : OwinMiddleware
    {
        public RespondWithNotFound(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            context.Response.ReasonPhrase = "There were no matching rules";
            context.Response.StatusCode = 404;
            await Next.Invoke(context);
        }
    }
}