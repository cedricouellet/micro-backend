using Framework;
using Framework.Http;
using System.Net;

/*
 * This project serves as an example for using this micro-framework
 */

var address = IPAddress.Parse("127.0.0.1");
var port = 9000;

var builder = new WebServerBuilder(address, port)
    .UseLogging(() => Console.WriteLine) // we can set the logging method (method that takes a string parameter)
    .UseRequestLimiting(1)
    .UseStaticFiles("/wwwroot") // we can host static files
    .UseExceptionHandler("/error"); // we can redirect unhandled exceptions to a route or static file

// We can manually map routes, or use attribute based routing (see Routes/ExampleItemsRouteGroup.cs)
builder.MapRoute(HttpMethods.Get, "/error", (RequestContext ctx) =>
{
    ctx.Log?.Invoke($"[/error]: {ctx.Exception}");
    return new Response(HttpStatusCodes.InternalServerError500);
});

var server = builder.Build();

await server.RunAsync();