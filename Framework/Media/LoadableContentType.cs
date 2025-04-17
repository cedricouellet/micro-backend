using Framework.Loaders;

namespace Framework.Media
{
    /// <summary>
    /// A loadable MIME content type.
    /// </summary>
    /// <param name="loader">The attributed file loader.</param>
    /// <param name="contentType">The MIME content type.</param>
    internal class LoadableContentType(IFileLoader loader, string contentType)
    {
        /// <summary>
        /// Gets the attributed file loader.
        /// </summary>
        public IFileLoader Loader { get; } = loader;

        /// <summary>
        /// Gets the MIME content type.
        /// </summary>
        public string ContentType { get; } = contentType;
    }
}
