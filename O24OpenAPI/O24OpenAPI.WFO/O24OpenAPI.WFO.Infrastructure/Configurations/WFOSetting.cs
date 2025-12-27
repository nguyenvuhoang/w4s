using O24OpenAPI.Core.Configuration;

namespace O24OpenAPI.WFO.Infrastructure.Configurations;

public class WFOSetting : ISettings
{
    public string? MessageBrokerHostName { get; set; }
    public int MessageBrokerPort { get; set; }
    public string? MessageBrokerUserName { get; set; }
    public string? MessageBrokerPassword { get; set; }
    public string? MessageBrokerPasswordEncryptionMethod { get; set; }
    public string? MessageBrokerVirtualHost { get; set; }
    public long ServerPingIntervalInSecond { get; set; }
    public string MessageBrokerSSLActive { get; set; } = "Y";
    public long MessageBrokerReconnectIntervalInSecond { get; set; }
    public string? RedisServerName { get; set; }
    public int RedisServerPort { get; set; }
    public long GrpcTimeoutInSeconds { get; set; } = 60;
    public int WorkflowTimeoutInSeconds { get; set; } = 60;
}
