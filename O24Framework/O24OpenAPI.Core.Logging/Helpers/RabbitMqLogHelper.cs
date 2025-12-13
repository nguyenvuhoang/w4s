using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using O24OpenAPI.Core.Domain;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Core.Logging.Enums;
using Serilog;

namespace O24OpenAPI.Core.Logging.Helpers;

public static class RabbitMqLogHelper
{
    public static void LogPublish<T>(
        string serviceName,
        string destination,
        T message,
        IDictionary<string, object> headers
    )
    {
        var correlationId = EngineContext.Current.Resolve<WorkContext>().ExecutionLogId;

        using (LogContextHelper.Push(correlationId, serviceName))
        {
            var payload = JsonConvert.SerializeObject(message, Formatting.Indented);

            Log.ForContext("LogType", LogType.RabbitMq)
                .ForContext("Direction", LogDirection.Out)
                .ForContext("Action", $"Publish to {destination}")
                .ForContext("Request", payload)
                .ForContext("Headers", JsonConvert.SerializeObject(headers, Formatting.Indented))
                .ForContext(
                    "Flow",
                    headers.TryGetValue("Flow", out var flowValue) ? flowValue.ToString() : null
                )
                .Information("RabbitMQ Publish Log");
        }
    }

    public static async Task LogConsumeAsync(
        string serviceName,
        string source,
        byte[] body,
        IDictionary<string, object> headers,
        Func<Task> processAction
    )
    {
        var correlationId = EngineContext.Current.Resolve<WorkContext>().ExecutionLogId;

        using (LogContextHelper.Push(correlationId, serviceName))
        {
            var stopwatch = Stopwatch.StartNew();
            string payload = Encoding.UTF8.GetString(body);
            Exception? exception = null;
            try
            {
                await processAction();
            }
            catch (Exception ex)
            {
                exception = ex;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                Log.ForContext("LogType", LogType.RabbitMq)
                    .ForContext("Direction", LogDirection.In)
                    .ForContext("Action", $"Consume from {source}")
                    .ForContext("Request", payload)
                    .ForContext("Error", exception)
                    .ForContext("Duration", stopwatch.ElapsedMilliseconds)
                    .ForContext(
                        "Headers",
                        JsonConvert.SerializeObject(headers, Formatting.Indented)
                    )
                    .Information("RabbitMQ Consume Log");
            }
        }
    }
}
