using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace Framework.Http
{
    /// <summary>
    /// Manages parsing the body for an HTTP request
    /// </summary>
    internal static partial class RequestBodyParser
    {
        /// <summary>
        /// Parses HTTP request body text into a dictionary.
        /// </summary>
        /// <param name="body">The body text to parse.</param>
        /// <param name="contentType">The content type of the request.</param>
        /// <returns>The parsed dictionary.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IDictionary<string, object?> Parse(string body, string? contentType)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                return new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            }

            if (contentType?.Equals(MediaTypeNames.Application.Json, StringComparison.OrdinalIgnoreCase) == true)
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(body);
                return dict?.ToDictionary(kv => kv.Key, kv => ConvertJsonElement(kv.Value), StringComparer.OrdinalIgnoreCase) ?? [];
            }

            if (contentType?.Equals(MediaTypeNames.Application.FormUrlEncoded, StringComparison.OrdinalIgnoreCase) == true)
            {
                return body.Split("&")
                   .Select(x => x.Split("=", 2)) // Ensure we split only on first '='
                   .ToDictionary(
                       key => Uri.UnescapeDataString(key[0]),
                       value => value.Length > 1 ? (object)Uri.UnescapeDataString(value[1]) : null
                   );
            }

            throw new ArgumentOutOfRangeException(nameof(contentType), "Content type not supported");
        }

        /// <summary>
        /// Converts a JSON element to a COM object.
        /// </summary>
        /// <param name="element">The JSON element to convert.</param>
        /// <returns>The resulting COM object, or null if the source element was null or undefined.</returns>
        private static object? ConvertJsonElement(JsonElement element)
        {
            return element.ValueKind switch
            {

                // HTML Encode is used as our sanitization method to prevent client-side injection attacks
                JsonValueKind.String => WebUtility.HtmlEncode(element.GetString())!, 
                
                JsonValueKind.Number => element.TryGetInt64(out long longValue) 
                                            ? longValue 
                                            : element.GetDouble(),
                
                JsonValueKind.True => true,
                
                JsonValueKind.False => false,
                
                JsonValueKind.Array => element.EnumerateArray()
                                              .Select(ConvertJsonElement)
                                              .ToArray(),
                
                JsonValueKind.Object => element.EnumerateObject()
                                               .ToDictionary(
                                                    name => name.Name, 
                                                    value => ConvertJsonElement(value.Value)),
                
                JsonValueKind.Undefined => null,
                
                JsonValueKind.Null => null,
                _ => null,
            };
        }
    }
}
