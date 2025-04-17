using Framework.Extensions;
using Framework.Routing;
using Framework.State;
using System.Net;

namespace Framework.Http
{
    /// <summary>
    /// A handler that manages HTTP contexts
    /// </summary>
    /// <param name="routeTable">A table of registered routes.</param>
    /// <param name="sessionDuration">The duration of a session.</param>
    /// <param name="staticRoot">The root of the static directory, if applicable</param>
    /// <param name="exceptionHandlerPath">The path to route to in case of unhandled exceptions, if applicable</param>
    /// <param name="log">The method that logs messages, if applicable</param>
    internal class HttpContextHandler(
        RouteTable routeTable,
        TimeSpan sessionDuration,
        string? staticRoot = null,
        string? exceptionHandlerPath = null,
        Action<string>? log = null)
    {
        /// <summary>
        /// The duration of a session
        /// </summary>
        private readonly TimeSpan _sessionDuration = sessionDuration;

        /// <summary>
        /// The router which routes the context's request to a resource or response
        /// </summary>
        private readonly Router _router = new(routeTable, staticRoot, exceptionHandlerPath, log);

        /// <summary>
        /// The session manager used to manage session
        /// </summary>
        private readonly SessionManager _sessionManager = new();

        /// <summary>
        /// The method that logs messages
        /// </summary>
        private readonly Action<string>? _log = log;

        /// <summary>
        /// Executes the handler on an HTTP context.
        /// </summary>
        /// <param name="httpContext">The HTTP context to handle.</param>
        public void Execute(HttpListenerContext httpContext)
        {
            _log?.Invoke($"[HttpContextHandler] {httpContext.Request.RemoteEndPoint} - {httpContext.Request.HttpMethod} {httpContext.Request.Url}");

            var session = _sessionManager.Get(httpContext.Request.RemoteEndPoint, (existing) => existing.IsExpired(_sessionDuration));

            var routerRequest = BuildRequest(httpContext);
            var routedResponse = _router.Route(routerRequest, session);

            WriteResponse(httpContext, routedResponse);
        }

        /// <summary>
        /// Builds a request using an HTTP context
        /// </summary>
        /// <param name="httpContext">The HTTP context from which to build the request.</param>
        /// <returns>The generated request.</returns>
        private static Request BuildRequest(HttpListenerContext httpContext)
        {
            IDictionary<string, object?> body = new Dictionary<string, object?>();
            if (httpContext.Request.HasEntityBody)
            {
                using var stream = httpContext.Request.InputStream;
                using var reader = new StreamReader(stream, httpContext.Request.ContentEncoding);

                var decodedBodyText = WebUtility.UrlDecode(reader.ReadToEnd());
                body = RequestBodyParser.Parse(decodedBodyText, httpContext.Request.ContentType);
            }

            return new Request(
                 httpContext.Request.HttpMethod,
                 httpContext.Request.Url!.LocalPath.TrimStart('/'),
                 httpContext.Request.QueryString.ToDictionary(),
                 body,
                 httpContext.Request.Headers.ToDictionary());
        }

        /// <summary>
        /// Writes a routed response to an HTTP context.
        /// </summary>
        /// <param name="httpContext">The HTTP context on which to write the response.</param>
        /// <param name="routedResponse">The routed response used to write to the HTTP context.</param>
        private static void WriteResponse(HttpListenerContext httpContext, Response routedResponse)
        {
            httpContext.Response.ContentType = routedResponse.Content?.ContentType;
            httpContext.Response.ContentLength64 = routedResponse.Content?.Data?.LongLength ?? 0;

            if (routedResponse.RedirectPath != null)
            {
                httpContext.Response.StatusCode = HttpStatusCodes.TemporaryRedirect307;
                httpContext.Response.Redirect($"http://{httpContext.Request.UserHostAddress}/{routedResponse.RedirectPath}");
                httpContext.Response.Close();
                return;
            }
            
            httpContext.Response.StatusCode = routedResponse.StatusCode;
            if (routedResponse.Content != null)
            {
                using (httpContext.Response.OutputStream)
                httpContext.Response.OutputStream.Write(routedResponse.Content.Data);
            }

            httpContext.Response.Close();
        }
    }
}
