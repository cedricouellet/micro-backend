using Framework.Http;
using System.Text;

namespace Framework.Loaders
{
    /// <summary>
    /// A file loader for text-based files.
    /// </summary>
    internal class TextFileLoader : IFileLoader
    {
        /// <summary>
        /// Loads a text byte array from a file.
        /// </summary>
        /// <param name="absolutePath">The absolute path of the file to load.</param>
        /// <returns>The text byte data loaded from the file.</returns>
        public byte[] Load(string absolutePath)
        {
            using var stream = new FileStream(absolutePath, FileMode.Open, FileAccess.Read);
            using var reader = new StreamReader(stream);

            var text = reader.ReadToEnd();
            
            return Encoding.UTF8.GetBytes(text);
        }
    }
}
