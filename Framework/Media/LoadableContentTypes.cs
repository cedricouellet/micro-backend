using Framework.Loaders;
using System.Net.Mime;

namespace Framework.Media
{
    /// <summary>
    /// Predefined loadable MIME content types.
    /// </summary>
    internal static class LoadableContentTypes
    {
        /// <summary>
        /// Gets the loadable content type for HTML (Hypertext Markup Language)
        /// </summary>
        public static LoadableContentType Html { get; } = new LoadableContentType(new TextFileLoader(), MediaTypeNames.Text.Html);
        
        /// <summary>
        /// Gets the loadable content type for CSS (Cascading Style Sheets)
        /// </summary>
        public static LoadableContentType Css { get; } = new LoadableContentType(new TextFileLoader(), MediaTypeNames.Text.Css);

        /// <summary>
        /// Gets the loadable content type for JavaScript
        /// </summary>
        public static LoadableContentType JavaScript { get; } = new LoadableContentType(new TextFileLoader(), MediaTypeNames.Text.JavaScript);

        /// <summary>
        /// Gets the loadable content type for JSON (JavaScript Object Notation)
        /// </summary>
        public static LoadableContentType Json { get; } = new LoadableContentType(new TextFileLoader(), MediaTypeNames.Application.Json);

        /// <summary>
        /// Gets the loadable content type for XML (Extensible Markup Language)
        /// </summary>
        public static LoadableContentType Xml { get; } = new LoadableContentType(new TextFileLoader(), MediaTypeNames.Application.Xml);

        /// <summary>
        /// Gets the loadable content type for plain text.
        /// </summary>
        public static LoadableContentType PlainText { get; } = new LoadableContentType(new TextFileLoader(), MediaTypeNames.Text.Plain);

        /// <summary>
        /// Gets the loadable content type for PNG (Portable Network Graphics)
        /// </summary>
        public static LoadableContentType Png { get; } = new(new ImageFileLoader(), MediaTypeNames.Image.Png);

        /// <summary>
        /// Gets the loadable content type for JPEF (Join Photographic Exports Group)
        /// </summary>
        public static LoadableContentType Jpeg { get; } = new(new ImageFileLoader(), MediaTypeNames.Image.Jpeg);

        /// <summary>
        /// Gets the loadable content type for GIF (Graphics Interchange Format)
        /// </summary>
        public static LoadableContentType Gif { get; } = new(new ImageFileLoader(), MediaTypeNames.Image.Gif);

        /// <summary>
        /// Gets the loadable content type for Bitmaps
        /// </summary>
        public static LoadableContentType Bitmap { get; } = new(new ImageFileLoader(), MediaTypeNames.Image.Bmp);

        /// <summary>
        /// Gets the loadable content type for SVG (Scalable Vector Graphics)
        /// </summary>
        public static LoadableContentType Svg { get; } = new(new ImageFileLoader(), MediaTypeNames.Image.Svg);

        /// <summary>
        /// Gets the loadable content type for ICO (icons)
        /// </summary>
        public static LoadableContentType Icon { get; } = new LoadableContentType(new ImageFileLoader(), MediaTypeNames.Image.Icon);

        private static readonly Dictionary<string, LoadableContentType> _map = new()
        {
            { ".html", Html },
            { ".htm", Html },
            { ".css", Css },
            { ".js", JavaScript },
            { ".json", Json },
            { ".xml", Xml },
            { ".txt", PlainText },
            { ".png", Png },
            { ".jpg", Jpeg },
            { ".jpeg", Jpeg },
            { ".gif", Gif },
            { ".bmp", Bitmap },
            { ".svg", Svg },
            { ".ico", Icon },
        };

        /// <summary>
        /// Gets a loadable content type by a file extension.
        /// </summary>
        /// <param name="fileExtension">The file extension used to retrieve a loadable content type.</param>
        /// <returns>The loadable content type if found, otherwise null.</returns>
        public static LoadableContentType? GetByFileExtension(string fileExtension)
        {
            _map.TryGetValue(fileExtension, out var extension);
            return extension;
        }
    }
}
