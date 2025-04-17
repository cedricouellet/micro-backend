using Framework.Http;

namespace Framework.Loaders
{
    /// <summary>
    /// A file loader for images.
    /// </summary>
    internal class ImageFileLoader : IFileLoader
    {
        /// <summary>
        /// Loads an image byte array from a file.
        /// </summary>
        /// <param name="absolutePath">The absolute path of the file to load.</param>
        /// <returns>The image byte data loaded from the file.</returns>
        public byte[] Load(string absolutePath)
        {
            using var inputStream = new FileStream(absolutePath, FileMode.Open, FileAccess.Read);
            using var outputStream = new MemoryStream();

            inputStream.CopyTo(outputStream);

            return outputStream.ToArray();
        }
    }
}
