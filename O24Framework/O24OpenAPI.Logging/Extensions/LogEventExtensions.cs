using O24OpenAPI.Contracts.Models.Log;
using Serilog.Events;

namespace O24OpenAPI.Logging.Extensions;

public static class LogEventExtensions
{
    public static LogEntryModel ToLogEntryModel(this LogEvent logEvent)
    {
        var logEntry = new LogEntryModel()
        {
            LogTimestamp = logEvent.Timestamp,
            LogLevel = logEvent.Level.ToString(),
            RenderedMessage = logEvent.RenderMessage(),
            Exception = logEvent.Exception?.ToString(),
            LogType = GetStringValue(logEvent, "LogType"),
            Direction = GetStringValue(logEvent, "Direction"),
            ActionName = GetStringValue(logEvent, "Action"),
            DurationMs = GetLongValue(logEvent, "Duration"),
            ServiceName = GetStringValue(logEvent, "ServiceName"),
            CorrelationId = GetStringValue(logEvent, "CorrelationId"),
            RequestPayload = GetStringValue(logEvent, "Request"),
            ResponsePayload = GetStringValue(logEvent, "Response"),
            Headers = GetStringValue(logEvent, "Headers"),
            Flow = GetStringValue(logEvent, "Flow"),
        };

        return logEntry;
    }

    private static string? GetStringValue(LogEvent logEvent, string key)
    {
        return logEvent.Properties.TryGetValue(key, out var prop) && prop is ScalarValue scalar
            ? scalar.Value?.ToString()
            : null;
    }

    private static long? GetLongValue(LogEvent logEvent, string key)
    {
        if (
            logEvent.Properties.TryGetValue(key, out var prop)
            && prop is ScalarValue scalar
            && scalar.Value is long longVal
        )
        {
            return longVal;
        }
        return null;
    }
}
