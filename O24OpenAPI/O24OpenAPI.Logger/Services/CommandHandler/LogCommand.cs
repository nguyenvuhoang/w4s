using System.Text.Json;
using MediatR;

namespace O24OpenAPI.Logger.Services.CommandHandler;

/// <summary>
/// The log command class
/// </summary>
/// <seealso cref="IRequest"/>
public class LogCommand<T> : IRequest
    where T : class
{
    /// <summary>
    /// Gets the value of the log data
    /// </summary>
    public T LogData { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogCommand{T}"/> class
    /// </summary>
    /// <param name="stringLog">The string log</param>
    public LogCommand(string stringLog)
    {
        LogData = JsonSerializer.Deserialize<T>(stringLog);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LogCommand{T}"/> class
    /// </summary>
    /// <param name="logData">The log data</param>
    public LogCommand(T logData)
    {
        LogData = logData;
    }
}
