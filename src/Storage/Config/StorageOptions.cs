using System.Collections.Generic;

namespace Storage.Config
{
    public class StorageOptions
    {
        public AzureOptions AzureStorage { get; set; }
        public List<S3Options> S3 { get; set; }
    }
}