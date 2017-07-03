using System.Net.Http;
using System.Text;
using Alexw.StubServer.Core;
using NUnit.Framework;

namespace Alexw.StubServer.Tests
{
    [TestFixture]
    public class StubServerTests
    {
        public SelfHostedStubServer Instance { get; }

        public StubServerTests(SelfHostedStubServer instance)
        {
            Instance = instance;
        }

        [Test]
        public void Beans()
        {
            Instance..Add(context =>
            {
                return context.Request.Uri.PathAndQuery.StartsWith("//hello//world");
            }, context =>
            {
                var bytes = Encoding.UTF8.GetBytes("hello world");
                context.Response.Body.Write(bytes, 0, bytes.Length);
            });

            using (var client = new HttpClient())
            {
                client.GetAsync(Instance.
            }
        }

        
    }
}
