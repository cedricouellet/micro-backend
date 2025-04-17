using System.Net;

namespace Framework.State
{
    /// <summary>
    /// A session dictionary for a remote endpoint.
    /// </summary>
    public class Session : Dictionary<string, string>
    {
        internal Session() { }

        /// <summary>
        /// Gets the last connection date and time, in UTC.
        /// </summary>
        internal DateTime LastConnectionUtc { get; private set; } = DateTime.UtcNow;

        /// <summary>
        /// Determines whether or not the session is expired based on a specified expiration span.
        /// </summary>
        /// <param name="expiration">The duration of a session before expiry.</param>
        /// <returns>True if the session is expired, otherwise false.</returns>
        internal bool IsExpired(TimeSpan expiration)
        {
            return (DateTime.UtcNow - LastConnectionUtc).TotalSeconds > expiration.TotalSeconds;
        }
    }

    /// <summary>
    /// A manager of sessions, used for tracking and retrieveing sessions.
    /// </summary>
    internal class SessionManager
    {
        protected readonly Dictionary<IPAddress, Session> _sessions = [];

        /// <summary>
        /// Retrieves a session for a remote endpoint, with the possibility to specify whether or not it should be invalidated and recreated.
        /// </summary>
        /// <param name="remoteEndPoint">The incoming request remote endpoint.</param>
        /// <param name="invalidate">
        /// The method used to specify whether or not a session should be invalidated. 
        /// This is only called if an existing session is found for the remote endpoint.
        /// </param>
        /// <returns>The resulting session.</returns>
        public Session Get(IPEndPoint remoteEndPoint, Func<Session, bool>? invalidate = null)
        {
            _sessions.TryGetValue(remoteEndPoint.Address, out var session);

            if (session != null && invalidate != null && invalidate(session))
            {
                _sessions.Remove(remoteEndPoint.Address);
                session = null;
            }

            if (session == null)
            {
                session = [];
                _sessions[remoteEndPoint.Address] = session;
            }

            return session;
        }
    }
}
