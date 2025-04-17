using Framework.State;

namespace Framework.Http
{
    /// <summary>
    /// Represents the context for an HTTP request.
    /// </summary>
    public class RequestContext
    {
        /// <summary>
        /// Instantiates a new request context.
        /// </summary>
        /// <param name="request">The request for the context.</param>
        /// <param name="session">A session for the context</param>
        internal RequestContext(Request request, Session session)
        {
            Request = request;
            Session = session;
        }

        /// <summary>
        /// Gets the request.
        /// </summary>
        public Request Request { get; }

        /// <summary>
        /// Gets the session.
        /// </summary>
        public Session Session { get; }

        /// <summary>
        /// Gets the log method.
        /// </summary>
        public Action<string>? Log { get; internal set; }

        /// <summary>
        /// Gets the routed unhandled exception.
        /// </summary>
        public Exception? Exception { get; internal set; }
    }
}
