using System.Text.Json;

namespace O24OpenAPI.Sample.Services.CommandHandler;

public class LogCommand<T>
    where T : class
{
    public T LogData { get; }

    public LogCommand(string stringLog)
    {
        LogData = JsonSerializer.Deserialize<T>(stringLog);
    }

    public LogCommand(T logData)
    {
        LogData = logData;
    }
}
