using Framework.Http;
using System.Text.RegularExpressions;

namespace Framework.Routing
{
    /// <summary>
    /// A route table for registering logical routes onto the web server, with support for basic route patterns (ex: /resource/{id})
    /// </summary>
    internal class RouteTable
    {
        private readonly Dictionary<Regex, Route> _routeDictionary = [];

        /// <summary>
        /// Registers a route.
        /// </summary>
        /// <param name="route">The logical route to register.</param>
        internal void RegisterRoute(Route route)
        {
            var regexPattern = "^" + Regex.Replace(route.Path, @"\{(\w+)\}", "(?<$1>[^/]+)") + "$";
            var regex = new Regex(regexPattern, RegexOptions.Compiled);

            _routeDictionary[regex] = route;  // Overwrites if route already exists
        }

        /// <summary>
        /// Registers multiple routes.
        /// </summary>
        /// <param name="routes">The logical routes to register.</param>
        internal void RegisterRoutes(IEnumerable<Route> routes)
        {
            foreach (var route in routes)
            {
                RegisterRoute(route);
            }
        }

        /// <summary>
        /// Matches a request to a logical route.
        /// </summary>
        /// <param name="request">The incoming request.</param>
        /// <returns>The route that best matches the request, or null if no route could be resolved for the path.</returns>
        internal Route? MatchRoute(Request request)
        {
            foreach (var (regex, route) in _routeDictionary.Where(x => x.Value.Method == request.Method).OrderByDescending(x => x.Value.Path.Contains('{')))
            {
                var match = regex.Match(request.Path.Trim('/'));
                if (match.Success)
                {
                    var parameters = match.Groups.Keys
                        .Where(k => !int.TryParse(k, out _))
                        .ToDictionary(k => k, k => match.Groups[k].Value);

                    request.RouteParams = parameters;
                    return route;
                }
            }

            return null;
        }
    }
}
