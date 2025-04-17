using Framework.Http;
using System.Reflection;

namespace Framework.Routing
{
    /// <summary>
    /// A scanner used for dynamically finding attribute-based routes.
    /// </summary>
    internal static class RouteScanner
    {
        /// <summary>
        /// Scans routes marked with <see cref="RouteAttribute"/>. For routed actions that are in a class marked with <seealso cref="RouteGroupAttribute"/>, the group's prefix is taken into account.
        /// </summary>
        /// <returns>An enumeration of routes.</returns>
        /// <exception cref="InvalidRouteAttributeUsage"></exception>
        public static IEnumerable<Route> ScanRoutes()
        {
            var routes = new List<Route>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var routeCollectionTypes = assembly.GetTypes()
               .Where(t => t.GetCustomAttribute<RouteGroupAttribute>() != null);

                foreach (var type in routeCollectionTypes)
                {
                    var routeCollectionAttribute = type.GetCustomAttribute<RouteGroupAttribute>()!;
                    var prefix = routeCollectionAttribute.Prefix;

                    var routeMethods = type
                        .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                        .Where(m => m.GetCustomAttribute<RouteAttribute>() != null);

                    foreach (var routeMethod in routeMethods)
                    {
                        routes.Add(ActivateRoute(routeMethod, prefix));
                    }
                }

                var orphanedRouteMethods = assembly.GetTypes().Where(t => t.GetCustomAttribute<RouteGroupAttribute>() == null)
                    .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance))
                    .Where(m => m.GetCustomAttribute<RouteAttribute>() != null && m.DeclaringType?.GetCustomAttribute<RouteGroupAttribute>() == null);

                foreach (var routeMethod in orphanedRouteMethods)
                {
                    routes.Add(ActivateRoute(routeMethod, null)); 
                }
            }

            return routes;
        }


        /// <summary>
        /// Activates a route using a route method info.
        /// </summary>
        /// <param name="routeMethod">The method info from which to activate a route.</param>
        /// <param name="prefix">The prefix for the route, usually derived from a route group.</param>
        /// <returns>The activated route.</returns>
        /// <exception cref="InvalidRouteAttributeUsage"></exception>
        private static Route ActivateRoute(MethodInfo routeMethod, string? prefix)
        {
            var routeAttribute = routeMethod.GetCustomAttribute<RouteAttribute>()!;

            var fullPath = (routeAttribute.IgnoreGroupPrefix
                ? routeAttribute.Path
                : $"{prefix}/{routeAttribute.Path}").Trim('/');

            if (routeMethod.ReturnType != typeof(Response))
            {
                throw new InvalidRouteAttributeUsage($"The return type of a route must be: {typeof(Response).FullName}");
            }

            object? declaringInstance = null;
            Func<RequestContext, Response> action;

            var methodParameters = routeMethod.GetParameters();

            if (methodParameters.Length > 1 || !methodParameters.All(x => x.ParameterType == typeof(RequestContext)))
            {
                throw new InvalidRouteAttributeUsage($"A route must may only accept a single parameter of type: {typeof(RequestContext).FullName}");
            }

            if (!routeMethod.IsStatic)
            {
                declaringInstance = Activator.CreateInstance(routeMethod.DeclaringType!);
            }

            action = (requestContext) =>
            {
                return (Response)routeMethod.Invoke(declaringInstance, methodParameters.Length == 0 ? null : [requestContext])!;
            };

            return new Route(routeAttribute.Method, fullPath, action);
        }
    }
}
