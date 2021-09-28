using System.IO;

namespace Storage.DTO
{
    public class StorageFile
    {
        /// <summary>
        /// File Content
        /// </summary>
        public Stream FileContentStream { get; set; }

        /// <summary>
        /// File ContentType
        /// </summary>
        public string ContentType { get; set; }
    }
}