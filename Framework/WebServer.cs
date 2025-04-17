using Framework.Http;
using Framework.Routing;
using System.Net;

namespace Framework
{
    /// <summary>
    /// A web server that listens to HTTP connections and dispatches them.
    /// </summary>
    public class WebServer 
    {
        private readonly HttpListener _listener;
        private readonly Semaphore? _semaphore;
        private readonly HttpContextHandler _handler;
        private readonly Action<string>? _log;

        /// <summary>
        /// Instantiates a new web server.
        /// </summary>
        /// <param name="baseAddress">The base address of the server</param>
        /// <param name="port">The port of the server.</param>
        /// <param name="routeTable">The route table registered for the server.</param>
        /// <param name="sessionDuration">The duration of a session.</param>
        /// <param name="staticRoot">The static file root directory, if applicable</param>
        /// <param name="exceptionHandlerPath">The path responsible for handling unhandled exceptions, if applicable.</param>
        /// <param name="connectionLimit">The maximum number of concurrent connections, if applicable.</param>
        /// <param name="log">The method used for logging messages, if applicable.</param>
        /// <exception cref="ArgumentException"></exception>
        internal WebServer(
            IPAddress baseAddress, 
            int port,
            RouteTable routeTable,
            TimeSpan sessionDuration,
            string? staticRoot, 
            string? exceptionHandlerPath,
            int? connectionLimit,
            Action<string>? log)
        {
            if (connectionLimit != null && connectionLimit < 0)
            {
                throw new ArgumentException("Value cannot be negative.", nameof(connectionLimit));
            }

            // Temporary, we only support static files for now
            if (staticRoot == null)
            {
                throw new ArgumentException("Only static file rendering is supported for now. Please call UseStaticFiles() on the ServerBuilder.");
            }

            if (staticRoot != null && !Directory.Exists(staticRoot))
            {
                throw new ArgumentException("Static root must be a valid existing directory.", nameof(staticRoot));
            }

            _log = log;
            
            _listener = new HttpListener();
            _listener.Prefixes.Clear();
            _listener.Prefixes.Add($"http://{baseAddress}:{port}/");

            if (connectionLimit != null)
            {
                _semaphore = new Semaphore(connectionLimit.Value, connectionLimit.Value);
            }
            
            _handler = new HttpContextHandler(
                routeTable, 
                sessionDuration, 
                staticRoot, 
                exceptionHandlerPath, 
                _log);
        }

        /// <summary>
        /// Runs the web server asynchronously to begin listening to incoming HTTP connections.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        public async Task RunAsync()
        {
            _listener.Start();

            _log?.Invoke($"[WebServer] Listening on {_listener.Prefixes.Single()}");

            while (_semaphore == null || _semaphore.WaitOne())
            {
                var httpContext = await _listener.GetContextAsync();

                _semaphore?.Release();

                var clientThread = new Thread(() =>
                {
                    _handler.Execute(httpContext);
                });

                clientThread.Start();
                clientThread.Join();
            }
        }
    }
}
