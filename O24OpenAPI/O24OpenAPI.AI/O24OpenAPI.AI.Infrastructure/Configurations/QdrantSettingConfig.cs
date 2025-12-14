using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.O24AI.Configuration
{
    public class QdrantSettingConfig : IConfig
    {
        public required string ClientName { get; set; }
        public required string Host { get; set; }
        public required int Port { get; set; }
        public int TimeoutInSeconds { get; set; }
    }
}
