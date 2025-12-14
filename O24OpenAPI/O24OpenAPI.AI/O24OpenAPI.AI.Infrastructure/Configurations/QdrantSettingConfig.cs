using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.O24AI.Configuration
{
    public class QdrantSettingConfig : IConfig
    {
        public string ClientName { get; set; }
        public string Endpoints { get; set; }
        public int TimeoutInSeconds { get; set; }
    }
}
