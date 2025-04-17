namespace Framework.Routing
{
    /// <summary>
    /// An exception denoting the invalid usage of a <see cref="RouteAttribute"/>.
    /// </summary>
    public class InvalidRouteAttributeUsage : Exception
    {
        internal InvalidRouteAttributeUsage(string message) : base(message) { }

        internal InvalidRouteAttributeUsage() : base("Invalid route attribute method.") { }
    }
}
