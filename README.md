# Alexw.StubServer
Create an in-memory stub server for testing or super simple hosting

# Using
Checkout the [src/Alexw.StubServer.Tests](src/Alexw.StubServer.Tests) folder for examples, but basically:

## Setup a json response at /hello/world
In this example, you see how to setup a server, run tests against it, and verify your the call.
You can use the servers RecordedCall collection to access calls (or to clear them).
You can manipulate calls based on the Rules collection, which can be ordered or updated.
Calls that don't have a match fall back to the NotFoundResponder, so you'll get a 404.

```csharp
[Test]
public async Task HelloWorld()
{
  using (var server = new Core.StubServer())
  {
    // return true if the rule should run
    // you have full access to the context here
    Func<IOwinContext, bool> matcher = context =>
    {
      return context.Request.Uri.PathAndQuery.StartsWith("/hello/world");
    };

    // what to do if the rule runs, like returning a string
    // again, full access to the context
    Action<IOwinContext> manipulator = context =>
    {
      var bytes = Encoding.UTF8.GetBytes("hello world");
      context.Response.Body.Write(bytes, 0, bytes.Length);
    };

    // rules can be added, deleted or updated
    server.Rules.Add(matcher, manipulator);

    // starting the server is super important!
    server.Start("http://localhost:" + TcpPorts.GetFreeTcpPort());

    // fetch via http
    using (var client = new HttpClient())
    {
      using (var response = await client.GetAsync(server.Address + "/hello/world?value=1"))
      {
        Assert.AreEqual(200, (int)response.StatusCode);
        Assert.AreEqual(1, server.Recorded.Count);
        Assert.AreEqual("/hello/world?value=1", server.Recorded[0].Uri.PathAndQuery);
      }
    }
  }
}
```
