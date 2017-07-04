using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Alexw.StubServer.Core;
using NUnit.Framework;

namespace Alexw.StubServer.Tests
{
    [TestFixture]
    public class StubServerTests
    {
        private Core.StubServer _instance;

        [SetUp]
        public void SetUp()
        {
            _instance = new Core.StubServer();
            _instance.Start("http://localhost:" + TcpPorts.GetFreeTcpPort());
        }

        [TearDown]
        public void TearDown()
        {
            _instance.Dispose();
        }

        [Test]
        public async Task RulesExists_ValidRequest_RequestManipulated()
        {
            _instance.Rules.Add(context =>
            {
                return context.Request.Uri.PathAndQuery.StartsWith(@"/hello/world");
            }, context =>
            {
                var bytes = Encoding.UTF8.GetBytes("hello world");
                context.Response.Body.Write(bytes, 0, bytes.Length);
            });

            using (var client = new HttpClient())
            {

                var response = await client.GetAsync(_instance.Address + @"/hello/world");

                Assert.AreEqual(200, (int) response.StatusCode);
            }
        }

        [Test]
        public async Task RuleDoesNotExist_ValidRequest_Returns404()
        {
            using (var client = new HttpClient())
            {

                var response = await client.GetAsync(_instance.Address + @"/does/not/match");

                Assert.AreEqual(404, (int)response.StatusCode);
            }
        }
    }
}
