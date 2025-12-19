using System.Text.Json;

namespace O24OpenAPI.O24Design.Services.CommandHandler;

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
