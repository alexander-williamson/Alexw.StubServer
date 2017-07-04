# Alexw.StubServer
Create an in-memory stub server for testing or super simple hosting

# Using
Checkout the [src/Tests](src/Tests) folder for examples, but basically:

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
  Action<IOwinContext> manipulator = context =>
  {
    var bytes = Encoding.UTF8.GetBytes("hello world");
    context.Response.Body.Write(bytes, 0, bytes.Length);
  };

  // rules can be added, deleted or updated (as this is just a Dictionary)
  _instance.Rules.Add(matcher, manipulator);

  // start the stub srever
  using (var server = new Core.StubServer())
  {
    // start the server at the address and port given (TcpPorts is a helper function provided in this library)
    server.Start("http://localhost:" + TcpPorts.GetFreeTcpPort());

    // create a new httpclient, and request the path using the address from the 
    using (var client = new HttpClient())
    {
      var response = await client.GetAsync(server.Address + @"/hello/world");
      Assert.AreEqual(200, (int) response.StatusCode);
    }

  } // kill the stub server
}
```
