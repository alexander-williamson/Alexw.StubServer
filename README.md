# Alexw.StubServer
Create an in-memory stub server for testing or super simple hosting

# Using
Checkout the [src/Alexw.StubServer.Tests](src/Alexw.StubServer.Tests) folder for examples, but basically:

## Setup a json response at /api/example
```csharp
[Test]
public async Task HelloWorld()
{
  // return true if the rule should run
  // you have full access to the context here
  Func<IOwinContext, bool> matcher = context =>
  {
    return context.Request.Uri.PathAndQuery.StartsWith(@"/hello/world");
  };

  // what to do if the rule runs, like returning a string
  // again, full access to the context
  Action<IOwinContext> manipulator = context =>
  {
    var bytes = Encoding.UTF8.GetBytes("hello world");
    context.Response.Body.Write(bytes, 0, bytes.Length);
  };
  
  // rules can be added, deleted or updated
  _instance.Rules.Add(matcher, manipulator);
  
  using (var server = new Core.StubServer())
  {
    // starting the server is super important!
    server.Start("http://localhost:" + TcpPorts.GetFreeTcpPort());

    // fetch via http
    using (var client = new HttpClient())
    {
      using(var response = await client.GetAsync(server.Address + @"/hello/world")) {
        Assert.AreEqual(200, (int) response.StatusCode);
      }
    }
  }
}
```
