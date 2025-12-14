using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.O24AI.Configuration
{
    public class QdrantSettingConfig : IConfig
    {
        public required string ClientName { get; set; }
        public required string Endpoints { get; set; }
        public int TimeoutInSeconds { get; set; }
    }
}
