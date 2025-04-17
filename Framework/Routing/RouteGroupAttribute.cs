namespace Framework.Routing
{
    /// <summary>
    /// An attribute for denoting a class as a routed action group (used to group routes with an optional prefix).
    /// </summary>
    /// <param name="method">The HTTP method for the route.</param>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RouteGroupAttribute : Attribute
    {
        private string? _prefix;

        /// <summary>
        /// Gets or sets the prefix of the route group. The prefix will later be appended to the routes unless they are configure to ignore this prefix.
        /// </summary>
        public string? Prefix
        {
            get => _prefix;
            set
            {
                if (_prefix != value)
                {
                    _prefix = value?.Trim('/');
                }
            }
        }
    }
}
