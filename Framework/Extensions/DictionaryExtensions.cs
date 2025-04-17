using System.Collections.Specialized;

namespace Framework.Extensions
{
    /// <summary>
    /// Extension methods related to <see cref="IDictionary{TKey, TValue}"/> types.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Attempts to retrieve a value from the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the dictionary's key.</typeparam>
        /// <typeparam name="TValue">The type of the dictionary's value.</typeparam>
        /// <param name="dictionary">The dictionary from which to retrieve a value.</param>
        /// <param name="key">The key for which to attempt to retrieve a value.</param>
        /// <returns>The value that was found, or null if it is not present in the dictionary.</returns>
        public static TValue? Get<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) 
        {
            dictionary.TryGetValue(key, out var value);
            return value;
        }

        /// <summary>
        /// Converts a <see cref="NameValueCollection"/> to a <see cref="IDictionary{String, String}"/>.
        /// </summary>
        /// <param name="collection">The name value collection to convert.</param>
        /// <returns>The resulting dictionary.</returns>
        internal static IDictionary<string, string?> ToDictionary(this NameValueCollection collection)
        {
            var dictionary = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

            foreach (string name in collection)
            {
                dictionary[name] = collection.Get(name);
            }

            return dictionary;
        }
    }
}
