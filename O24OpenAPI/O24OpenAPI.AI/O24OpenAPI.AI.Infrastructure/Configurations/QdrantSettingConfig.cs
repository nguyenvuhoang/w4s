using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.AI.Infrastructure.Configurations;

public class QdrantSettingConfig : IConfig
{
    public required string ClientName { get; set; }
    public required string Host { get; set; }
    public required int Port { get; set; }
    public int TimeoutInSeconds { get; set; }
}
