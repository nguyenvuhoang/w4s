using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.Web.CMS.Configuration
{
    public class StorageConfiguration : IConfig
    {
        public string Provider { get; set; } = "S3"; // or "Local", "AzureBlob", etc.
        public S3StorageConfiguration S3 { get; set; } = new S3StorageConfiguration();
    }

    public class S3StorageConfiguration
    {
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string BucketName { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
    }
}
