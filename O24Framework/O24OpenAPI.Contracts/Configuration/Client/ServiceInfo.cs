using System.ComponentModel.DataAnnotations;

namespace O24OpenAPI.Contracts.Configuration.Client;

public class ServiceInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceInfo"/> class
    /// </summary>
    public ServiceInfo() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceInfo"/> class
    /// </summary>
    /// <param name="_config">The config</param>
    public ServiceInfo(ServiceInfoConfiguration _config)
    {
        service_code = _config.ServiceCode;
        service_name = _config.ServiceName;
        service_grpc_url = _config.ServiceGrpcUrl;
        service_status = _config.ServiceStatus;
        service_grpc_active = _config.ServiceGrpcActive;
        service_grpc_timeout_seconds = _config.ServiceGrpcTimeoutSeconds;
        service_ping_interval_seconds = _config.ServicePingIntervalSeconds;
        service_static_token = _config.ServiceStaticToken;
        broker_virtual_host = _config.BrokerVirtualHost;
        broker_hostname = _config.BrokerHostname;
        broker_user_name = _config.BrokerUsername;
        broker_user_password = _config.BrokerPassword;
        broker_queue_name = _config.BrokerQueueName;
        event_queue_name = _config.EventQueueName;
        broker_port = _config.BrokerPort;
        ssl_active = _config.SslActive;
        ssl_cert_pass_pharse = _config.SslCertPassPhrase;
        ssl_cert_servername = _config.SslCertServerName;
        ssl_cert_base64 = _config.SslCertBase64;
        concurrent_threads = _config.ConcurrentThreads;
        broker_reconnect_interval_in_seconds = _config.BrokerReconnectIntervalInSeconds;
        grpc_Max_Send_Message_Size_In_MB = _config.GrpcMaxSendMessageSizeInMB;
        grpc_Max_Receive_Message_Size_In_MB = _config.GrpcMaxReceiveMessageSizeInMB;
        log_write_grpc_log = _config.LogWriteGrpcLog;
        redis_server_name = _config.RedisServerName;
        redis_server_port = _config.RedisServerPort;
        is_tracking = _config.IsTracking;
    }

    /// <summary>
    /// The enum service props enum
    /// </summary>
    public enum EnumServiceProps
    {
        /// <summary>
        /// The neptune version enum service props
        /// </summary>
        NeptuneVersion,

        /// <summary>
        /// The neptune client version enum service props
        /// </summary>
        NeptuneClientVersion,

        /// <summary>
        /// The web frame work version enum service props
        /// </summary>
        WebFrameWorkVersion,
    }

    /// <summary>
    /// The wfo grpc timeout in seconds
    /// </summary>
    public readonly long WFO_GRPC_TIMEOUT_IN_SECONDS = 60L;

    /// <summary>
    /// Gets or sets the value of the service code
    /// </summary>
    [Key]
    public string service_code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the service name
    /// </summary>
    public string service_name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the service grpc url
    /// </summary>
    public string service_grpc_url { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the service status
    /// </summary>
    public string service_status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the service grpc active
    /// </summary>
    public string service_grpc_active { get; set; } = "active";

    /// <summary>
    /// Gets or sets the value of the service grpc timeout seconds
    /// </summary>
    public long service_grpc_timeout_seconds { get; set; }

    /// <summary>
    /// Gets or sets the value of the service ping interval seconds
    /// </summary>
    public long service_ping_interval_seconds { get; set; }

    /// <summary>
    /// Gets or sets the value of the service static token
    /// </summary>
    public string? service_static_token { get; set; }

    /// <summary>
    /// Gets or sets the value of the broker virtual host
    /// </summary>
    public string broker_virtual_host { get; set; } = string.Empty;
    private string _brokerHostname = string.Empty;

    /// <summary>
    /// Gets or sets the value of the broker hostname
    /// </summary>
    public string broker_hostname
    {
        get => _brokerHostname;
        set => _brokerHostname = value;
    }

    /// <summary>
    /// Gets or sets the value of the broker user name
    /// </summary>
    public string broker_user_name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the broker user password
    /// </summary>
    public string broker_user_password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the broker queue name
    /// </summary>
    public string broker_queue_name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the event queue name
    /// </summary>
    public string event_queue_name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the broker port
    /// </summary>
    public int broker_port { get; set; }

    /// <summary>
    /// Gets or sets the value of the o24openapi grpc url
    /// </summary>
    public string o24openapi_grpc_url { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the value of the o24openapi server time
    /// </summary>
    public long o24openapi_server_time { get; set; }

    /// <summary>
    /// Gets or sets the value of the o24openapi grpc timeout seconds
    /// </summary>
    public long o24openapi_grpc_timeout_seconds { get; set; } = 60;

    public long workflow_execution_timeout_seconds { get; set; } = 300;

    /// <summary>
    /// Gets or sets the value of the ssl active
    /// </summary>
    public string? ssl_active { get; set; }

    /// <summary>
    /// Gets or sets the value of the ssl cert pass pharse
    /// </summary>
    public string? ssl_cert_pass_pharse { get; set; }

    /// <summary>
    /// Gets or sets the value of the ssl cert servername
    /// </summary>
    public string? ssl_cert_servername { get; set; }

    /// <summary>
    /// Gets or sets the value of the ssl cert base64
    /// </summary>
    public string? ssl_cert_base64 { get; set; }

    /// <summary>
    /// Gets or sets the value of the concurrent threads
    /// </summary>
    public long concurrent_threads { get; set; }

    /// <summary>
    /// Gets or sets the value of the broker reconnect interval in seconds
    /// </summary>
    public long broker_reconnect_interval_in_seconds { get; set; }

    /// <summary>
    /// Gets or sets the value of the grpc max send message size in mb
    /// </summary>
    public int grpc_Max_Send_Message_Size_In_MB { get; set; } = 4;

    /// <summary>
    /// Gets or sets the value of the grpc max receive message size in mb
    /// </summary>
    public int grpc_Max_Receive_Message_Size_In_MB { get; set; } = 4;

    /// <summary>
    /// Gets or sets the value of the log write grpc log
    /// </summary>
    public string log_write_grpc_log { get; set; } = "N";

    /// <summary>
    /// Gets or sets the value of the redis server name
    /// </summary>
    public string redis_server_name { get; set; } = "";

    /// <summary>
    /// Gets or sets the value of the redis server port
    /// </summary>
    public int redis_server_port { get; set; } = 6379;

    /// <summary>
    /// Gets or sets the value of the is tracking
    /// </summary>
    public bool is_tracking { get; set; }

    /// <summary>
    /// Gets or sets the value of the props
    /// </summary>
    public Dictionary<EnumServiceProps, string> props { get; set; } = [];
}
