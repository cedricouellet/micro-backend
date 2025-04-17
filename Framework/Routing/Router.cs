using Framework.Http;
using Framework.Media;
using Framework.State;

namespace Framework.Routing
{
    /// <summary>
    /// A router that is used to route requests.
    /// </summary>
    /// <param name="routeTable">The route table containing all registered routes.</param>
    /// <param name="staticRoot">The static file root directory, if applicable.</param>
    /// <param name="exceptionHandlerPath">The path responsible for handling unhandle exceptions, if applicable.</param>
    /// <param name="log">The method for logging messages, if applicable.</param>
    internal class Router(RouteTable routeTable, string? staticRoot = null, string? exceptionHandlerPath = null, Action<string>? log = null)
    {
        private readonly RouteTable _routeTable = routeTable;

        private readonly string? _staticRoot = staticRoot;

        private readonly string? _exceptionHandlerPath = exceptionHandlerPath;

        private readonly Action<string>? _log = log;

        /// <summary>
        /// Routes a request to a resource or route, then resolves it, returning its response.
        /// </summary>
        /// <param name="request">The incoming request.</param>
        /// <param name="session">The connected host's session.</param>
        /// <returns>The routed response.</returns>
        public Response Route(Request request, Session session)
        {
            var route = _routeTable.MatchRoute(request);

            if (route == null)
            {
                if (IsStaticResourceRequest(request.Method, request.Path))
                {
                    return LoadStaticResource(request.Path);
                }

                return new Response(HttpStatusCodes.NotFound404);
            }

            try
            {
                return route.Action(new RequestContext(request, session)
                {
                    Log = _log,
                });
            }
            catch (Exception ex)
            {
                if (_exceptionHandlerPath == null)
                {
                    throw;
                }

                var newRequest = new Request(HttpMethods.Get, _exceptionHandlerPath, request.QueryParams, request.Body, request.Headers);
                var errorRoute = _routeTable.MatchRoute(newRequest);

                if (errorRoute == null)
                {
                    return Response.Redirect(_exceptionHandlerPath);
                }

                return errorRoute.Action(new RequestContext(newRequest, session)
                {
                    Exception = ex,
                    Log = _log,
                });
            }
        }

        /// <summary>
        /// Loads a static resource.
        /// </summary>
        /// <param name="path">The relative path to the resource.</param>
        /// <returns>The resulting response.</returns>
        private Response LoadStaticResource(string path)
        {
            if (_staticRoot == null)
            {
                return new Response(HttpStatusCodes.NotFound404);
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                path = "index.html";
            }

            var mediaType = LoadableContentTypes.Html;

            if (Path.HasExtension(path))
            {
                mediaType = LoadableContentTypes.GetByFileExtension(Path.GetExtension(path));
            }

            if (mediaType == null)
            {
                return new Response(HttpStatusCodes.InternalServerError500);
            }

            string absolutePath = Path.Combine(_staticRoot, path);

            if (!File.Exists(absolutePath))
            {
                return new Response(HttpStatusCodes.NotFound404);
            }

            var content = new Content(mediaType.Loader.Load(absolutePath), mediaType.ContentType);
            return new Response(HttpStatusCodes.Ok200, content);
        }

        /// <summary>
        /// Determines whether or not a request path is requesting a static resource.
        /// </summary>
        /// <param name="path">The request path to check.</param>
        /// <returns>True if the request path is requesting a static resource, otherwise false.</returns>
        private static bool IsStaticResourceRequest(string method, string path)
        {
            if (method != HttpMethods.Get)
            {
                return false;
            }

            return string.IsNullOrWhiteSpace(path) || Path.HasExtension(path);
        }
    }
}
