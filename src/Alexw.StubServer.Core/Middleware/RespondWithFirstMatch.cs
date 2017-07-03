using System.Threading.Tasks;
using Microsoft.Owin;

namespace Alexw.StubServer.Core.Middleware
{
    public class RespondWithFirstMatch : OwinMiddleware
    {
        private readonly Rules _rules;

        public RespondWithFirstMatch(OwinMiddleware next, Rules rules) : base(next)
        {
            _rules = rules;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var match = _rules.GetFirstMatch(context);
            if (match != null)
            {
                match(context);
            }
            else
            {
                await Next.Invoke(context);
            }
        }
    }
}