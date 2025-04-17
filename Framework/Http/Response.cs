using System.Text.Json;
using System.Text;
using Framework.Media;

namespace Framework.Http
{
    /// <summary>
    /// Represents a response for an HTTP request.
    /// </summary>
    /// <param name="statusCode">The HTTP status code returned by the response.</param>
    public class Response(int statusCode)
    {
        /// <summary>
        /// Instantiates a new response.
        /// </summary>
        /// <param name="statusCode">The HTTP status code returned by the response.</param>
        /// <param name="content">The content sent by the response.</param>
        public Response(int statusCode, Content content) : this(statusCode)
        {
            Content = content;
        }

        /// <summary>
        /// Gets the HTTP status code.
        /// </summary>
        public int StatusCode { get; } = statusCode;

        /// <summary>
        /// Gets the content.
        /// </summary>
        public Content? Content { get; }

        /// <summary>
        /// Gets the path to redirect to.
        /// </summary>
        internal string? RedirectPath { get; private set; }

        /// <summary>
        /// Creates a JSON response.
        /// </summary>
        /// <param name="statusCode">The HTTP status code returned by the response.</param>
        /// <param name="data">The JSON data sent by the response.</param>
        /// <returns>The generated JSON response.</returns>
        public static Response Json(int statusCode, object data)
        {
            var json = JsonSerializer.Serialize(data);

            var bytes = Encoding.UTF8.GetBytes(json);

            var content = new Content(bytes, LoadableContentTypes.Json.ContentType);

            return new Response(statusCode, content);
        }

        /// <summary>
        /// Creates a redirection response.
        /// </summary>
        /// <param name="path">The path (route or resource) to redirect to.</param>
        /// <returns>The generated redirection response.</returns>
        public static Response Redirect(string path)
        {
            return new Response(HttpStatusCodes.TemporaryRedirect307)
            {
                RedirectPath = path,
            };
        }

        /// <summary>
        /// Creates an HTML response.
        /// </summary>
        /// <param name="htmlText">The HTML text sent by the response.</param>
        /// <returns>The generated HTML response.</returns>
        public static Response Html(string htmlText)
        {
            var bytes = Encoding.UTF8.GetBytes(htmlText);

            var content = new Content(bytes, LoadableContentTypes.Html.ContentType);

            return new Response(HttpStatusCodes.Ok200, content);
        }
    }
}
