using System.Collections.ObjectModel;

namespace Framework.Http
{
    /// <summary>
    /// A preconstructed HTTP Request for the web server.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// Instantiates a new request.
        /// </summary>
        /// <param name="method">The HTTP method of the request.</param>
        /// <param name="path">The path to a resource or route for the request.</param>
        /// <param name="queryParams">The query parameter dictionary for the request.</param>
        /// <param name="body">The body dictionary for the request.</param>
        /// <param name="headers">The header dictionary for the request.</param>
        internal Request(
            string method,
            string path,
            IDictionary<string, string?> queryParams,
            IDictionary<string, object?> body,
            IDictionary<string, string?> headers)
        {
            Method = method;
            Path = path;
            QueryParams = new ReadOnlyDictionary<string, string?>(queryParams);
            Headers = new ReadOnlyDictionary<string, string?>(headers);
            Body = new ReadOnlyDictionary<string, object?>(body);
            RouteParams = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
        }

        /// <summary>
        /// Gets the HTTP method.
        /// </summary>
        public string Method { get; } 

        /// <summary>
        /// Gets the path to a route or resource.
        /// </summary>
        public string Path { get; } 

        /// <summary>
        /// Gets the route parameter dictionary.
        /// </summary>
        public IDictionary<string, string> RouteParams { get; internal set; }

        /// <summary>
        /// Gets the query parameter dictionary.
        /// </summary>
        public IDictionary<string, string?> QueryParams { get; } 

        /// <summary>
        /// Gets the header dictionary.
        /// </summary>
        public IDictionary<string, string?> Headers { get; } 

        /// <summary>
        /// Gets the body dictionary.
        /// </summary>
        public IDictionary<string, object?> Body { get; }
    }
}
