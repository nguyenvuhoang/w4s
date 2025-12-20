using Serilog.Events;

namespace O24OpenAPI.Logging.Abstractions;

public interface ILogSubmitter : IDisposable
{
    Task SubmitAsync(IEnumerable<LogEvent> logEvents);
}
