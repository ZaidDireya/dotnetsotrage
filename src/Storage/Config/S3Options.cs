namespace Storage.Config
{
    public class S3Options
    {
        public string BucketName { set; get; }
        public string SecretKey { set; get; }
        public string AccessKey { set; get; }
        public string Region { set; get; }

        public bool IsConfigurationValid()
        {
            return !string.IsNullOrEmpty(BucketName) && !string.IsNullOrEmpty(SecretKey) && !string.IsNullOrEmpty(AccessKey) && !string.IsNullOrEmpty(Region);
        }
    }
}