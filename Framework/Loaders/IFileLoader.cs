namespace Framework.Loaders
{
    /// <summary>
    /// Represents a file loader.
    /// </summary>
    internal interface IFileLoader
    {
        /// <summary>
        /// Loads a byte array from a file.
        /// </summary>
        /// <param name="absolutePath">The absolute path of the file to load.</param>
        /// <returns>The byte data loaded from the file.</returns>
        byte[] Load(string absolutePath);
    }
}
