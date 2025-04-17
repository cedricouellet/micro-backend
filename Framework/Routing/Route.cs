using Framework.Http;

namespace Framework.Routing
{
    /// <summary>
    /// A logical route in the web server.
    /// </summary>
    /// <param name="method">The HTTP method of the route.</param>
    /// <param name="path">The path mapped to the route.</param>
    /// <param name="action">The action handling the route.</param>
    public class Route(string method, string path, Func<RequestContext, Response> action)
    {
        /// <summary>
        /// Gets the path mapped to the route.
        /// </summary>
        public string Path { get; } = path.Trim('/');

        /// <summary>
        /// Gets the HTTP action of the route.
        /// </summary>
        public string Method { get; } = method;

        /// <summary>
        /// Gets the action handling the route.
        /// </summary>
        public Func<RequestContext, Response> Action { get; } = action;
    }
}
