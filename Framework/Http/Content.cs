namespace Framework.Http
{
    /// <summary>
    /// Represents content that is inserted into an HTTP Response
    /// </summary>
    /// <param name="data">The byte array representing the data.</param>
    /// <param name="contentType">The MIME content type.</param>
    public class Content(byte[] data, string contentType)
    {
        /// <summary>
        /// Gets the byte array representing the data.
        /// </summary>
        public byte[] Data { get; } = data;

        /// <summary>
        /// Gets the MIME content type.
        /// </summary>
        public string ContentType { get; } = contentType;
    }
}
