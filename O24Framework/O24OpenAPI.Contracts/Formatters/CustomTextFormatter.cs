using Serilog.Events;
using Serilog.Formatting;
using System.Text;

namespace O24OpenAPI.Contracts.Formatters;

public class CustomTextFormatter : ITextFormatter
{
    public void Format(LogEvent logEvent, TextWriter output)
    {
        bool isBlockLog = logEvent.Properties.ContainsKey("Direction");

        if (isBlockLog)
        {
            FormatBlockLog(logEvent, output);
        }
        else
        {
            FormatSimpleLog(logEvent, output);
        }
    }

    private static void FormatSimpleLog(LogEvent logEvent, TextWriter output)
    {
        string serviceName = GetPropertyValue(logEvent, "ServiceName");
        string traceId = GetPropertyValue(logEvent, "CorrelationId");

        output.Write($"[{logEvent.Timestamp:yyyy-MM-dd HH:mm:ss.fff}] ");
        output.Write($"[{logEvent.Level.ToString().ToUpper()}] ");
        output.Write($"[{serviceName}] ");
        if (!string.IsNullOrEmpty(traceId))
        {
            output.Write($"[{traceId}] ");
        }
        logEvent.RenderMessage(output);

        if (logEvent.Exception != null)
        {
            output.WriteLine();
            output.Write(logEvent.Exception.ToString());
        }
        output.WriteLine();
    }

    private static void FormatBlockLog(LogEvent logEvent, TextWriter output)
    {
        var sb = new StringBuilder();
        sb.AppendLine("----------------------------------------");

        string serviceName = GetPropertyValue(logEvent, "ServiceName");
        string action = GetPropertyValue(logEvent, "Action");
        string traceId = GetPropertyValue(logEvent, "CorrelationId");
        string request = GetPropertyValue(logEvent, "Request");
        string response = GetPropertyValue(logEvent, "Response");
        var error = logEvent.Exception;
        string duration = GetPropertyValue(logEvent, "Duration");
        string headers = GetPropertyValue(logEvent, "Headers");

        sb.AppendLine($"[BEGIN] {serviceName} | {action}");
        sb.AppendLine($"[Time  ] {logEvent.Timestamp:yyyy-MM-dd HH:mm:ss.fff}");
        sb.AppendLine($"[Trace ] {traceId}");
        sb.AppendLine($"[Headers] {headers}");

        if (!string.IsNullOrEmpty(request))
        {
            sb.AppendLine();
            sb.AppendLine("[REQUEST]");
            sb.AppendLine(request);
        }

        if (error != null)
        {
            sb.AppendLine();
            sb.AppendLine("[ERROR]");
            sb.AppendLine(error.ToString());
        }
        else if (!string.IsNullOrEmpty(response))
        {
            sb.AppendLine();
            sb.AppendLine("[RESPONSE]");
            sb.AppendLine(response);
        }

        sb.AppendLine();
        if (error != null)
        {
            sb.AppendLine($"[END] Failed after {duration} ms");
        }
        else
        {
            sb.AppendLine($"[END] Duration: {duration} ms");
        }

        sb.AppendLine("----------------------------------------");
        output.Write(sb.ToString());
    }

    private static string GetPropertyValue(LogEvent logEvent, string propertyName)
    {
        if (
            logEvent.Properties.TryGetValue(propertyName, out var propertyValue)
            && propertyValue is Serilog.Events.ScalarValue scalarValue
        )
        {
            return scalarValue.Value?.ToString() ?? string.Empty;
        }
        return string.Empty;
    }
}
