namespace Framework.Routing
{
    /// <summary>
    /// An attribute for denoting a method as a routed action (will be registered as a logical route).
    /// </summary>
    /// <param name="method">The HTTP method for the route.</param>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class RouteAttribute(string method) : Attribute
    {
        private string _path = "";

        /// <summary>
        /// Gets or sets the path of the route.
        /// </summary>
        public string Path
        {
            get => _path;
            set
            {
                if (_path != value)
                {
                    _path = value.Trim('/');
                }
            }
        }

        /// <summary>
        /// Gets or sets whether or not to ignore the route group's prefix.
        /// <para>This is only applied if the route is in a class denoted by a <see cref="RouteGroupAttribute"/> with a prefix.</para>
        /// </summary>
        public bool IgnoreGroupPrefix { get; set; } = false;

        /// <summary>
        /// Gets the HTTP method for the route.
        /// </summary>
        public string Method { get; } = method;
    }
}
