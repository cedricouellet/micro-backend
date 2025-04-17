using Framework.Http;
using Framework.Routing;
using System.Net;

namespace Framework
{
    /// <summary>
    /// A builder for creating web servers.
    /// </summary>
    /// <param name="baseAddress">The base address of the web server.</param>
    /// <param name="port">The port of the web server.</param>
    public class WebServerBuilder(IPAddress baseAddress, int port)
    {
        private readonly IPAddress baseAddress = baseAddress;
        private readonly int port = port;
        private readonly RouteTable _routeTable = new();

        private string? staticRoot;
        private Action<string>? log;
        private int? connectionLimit;
        private string? exceptionHandlerPath;
        private TimeSpan sessionDuration = new(0, 20, 0);

        /// <summary>
        /// Adds static file handling to the web server.
        /// </summary>
        /// <param name="staticRoot">The static file directory relative to the project directory.</param>
        /// <returns>The builder instance.</returns>
        public WebServerBuilder UseStaticFiles(string staticRoot = "wwwroot")
        {
            this.staticRoot = Path.Combine(Directory.GetCurrentDirectory(), staticRoot.Trim('/'));
            return this;
        }

        /// <summary>
        /// Registers a method for logging messages.
        /// </summary>
        /// <param name="logFactory">The factory resulting in a log method.</param>
        /// <returns>The builder instance.</returns>
        public WebServerBuilder UseLogging(Func<Action<string>> logFactory)
        {
            log = logFactory();
            return this;
        }

        /// <summary>
        /// Registers a method for logging messages.
        /// </summary>
        /// <param name="log">The log method.</param>
        /// <returns>The builder instance.</returns>
        public WebServerBuilder UseLogging(Action<string> log)
        {
            this.log = log;
            return this;
        }

        /// <summary>
        /// Sets a limit to the number of concurrent connections allowed by the web server.
        /// </summary>
        /// <param name="connectionLimitFactory">The connection limit factory method.</param>
        /// <returns>The builder instance.</returns>
        public WebServerBuilder UseConnectionLimiting(Func<int> connectionLimitFactory)
        {
            connectionLimit = connectionLimitFactory();
            return this;
        }

        /// <summary>
        /// Sets a limit to the number of concurrent connections allowed by the web server.
        /// <para>
        /// Please note: this does not mean that only <c>N</c> concurrent users may call the server. 
        /// Rather, <c>N</c> requests must be fulfilled before handling <c>1..N</c> more requests.
        /// Use this in case of memory limitations where handling too many requests at once could cause a bottleneck.
        /// </para>
        /// </summary>
        /// <param name="connectionLimit">The maximum number of concurrent connections.</param>
        /// <returns>The builder instance.</returns>
        public WebServerBuilder UseRequestLimiting(int connectionLimit)
        {
            this.connectionLimit = connectionLimit;
            return this;
        }

        /// <summary>
        /// Maps a logical route in the web server route table.
        /// </summary>
        /// <param name="method">The HTTP method</param>
        /// <param name="path">The relative path</param>
        /// <param name="action">The action executed on the route</param>
        /// <returns>The builder instance.</returns>
        public WebServerBuilder MapRoute(string method, string path, Func<RequestContext, Response> action)
        {
            _routeTable.RegisterRoute(new Route(method, path, action));
            return this;
        }

        /// <summary>
        /// Registers the path to redirect to when encountering unhandled exceptions
        /// </summary>
        /// <param name="path">A relative path pointing to a static resource or route.</param>
        /// <returns>The builder instance.</returns>
        public WebServerBuilder UseExceptionHandler(string path)
        {
            exceptionHandlerPath = path.Trim('/');
            return this;
        }

        /// <summary>
        /// Overrides the default session duration (time until expiry).
        /// <para>The default session value is 20 minutes.</para>
        /// </summary>
        /// <param name="sessionDuration">The session duration.</param>
        /// <returns>The builder instance.</returns>
        public WebServerBuilder OverrideSessionDuration(TimeSpan sessionDuration)
        {
            this.sessionDuration = sessionDuration;
            return this;
        }

        /// <summary>
        /// Builds the web server.
        /// <para>Classes marked with [<see cref="RouteGroupAttribute"/>] as well as methods marked with [<see cref="RouteAttribute"/>] are scanned at this time.</para>
        /// </summary>
        /// <returns>The created web server instance.</returns>
        /// <exception cref="Exception"></exception>
        public WebServer Build()
        {
            try
            {
                var routes = RouteScanner.ScanRoutes();
                _routeTable.RegisterRoutes(routes);
            }
            catch (InvalidRouteAttributeUsage ex)
            {
                throw new Exception(ex.Message);
            }

            return new WebServer(
                baseAddress, 
                port,
                _routeTable,
                sessionDuration,
                staticRoot: staticRoot,
                exceptionHandlerPath: exceptionHandlerPath,
                connectionLimit: connectionLimit,
                log: log);
        }
    }
}
