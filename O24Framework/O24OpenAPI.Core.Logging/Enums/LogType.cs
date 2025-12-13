namespace O24OpenAPI.Core.Logging.Enums;

/// <summary>
/// Defines the type of communication channel being logged.
/// </summary>
public enum LogType
{
    RestApi,
    Grpc,
    RabbitMq,
    Business,
    ScheduleTask,
}
