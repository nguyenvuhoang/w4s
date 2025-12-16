using Linh.JsonKit.Json;
using O24OpenAPI.Logging.Enums;
using Serilog;

namespace O24OpenAPI.Logging.Helpers;

/// <summary>
/// Provides helper methods for logging general Business or Business events.
/// </summary>
public static class BusinessLogHelper
{
    /// <summary>
    /// Logs an informational Business event.
    /// This log will not have the standard [BEGIN]/[END] block format.
    /// It's a simple, single-line message enriched with context.
    /// </summary>
    /// <param name="messageTemplate">The message template, e.g., "User {UserId} completed action {ActionName}".</param>
    /// <param name="args">The arguments for the message template.</param>
    public static void Info(string messageTemplate, params object?[] args)
    {
        Log.ForContext("LogType", LogType.Business).Information(messageTemplate, args);
    }

    /// <summary>
    /// Logs a warning event.
    /// </summary>
    public static void Warning(string messageTemplate, params object?[] args)
    {
        Log.ForContext("LogType", LogType.Business).Warning(messageTemplate, args);
    }

    /// <summary>
    /// Logs an error event.
    /// </summary>
    public static void Error(Exception ex, string messageTemplate, params object?[] args)
    {
        Log.ForContext("LogType", LogType.Business).Error(ex, messageTemplate, args);
    }

    public static void WriteError(this Exception ex, string messageTemplate, params object?[] args)
    {
        Log.ForContext("LogType", LogType.Business).Error(ex, messageTemplate, args);
    }

    /// <summary>
    /// Logs a more detailed Business process with a clear start and end.
    /// This uses the standard [BEGIN]/[END] block format.
    /// </summary>
    /// <param name="actionName">A descriptive name for the Business action, e.g., "Calculating order total".</param>
    /// <param name="processAction">The Business logic action to be executed and timed.</param>
    /// <param name="inputData">Optional: An object representing the input data for the action, will be serialized to JSON.</param>
    public static void LogProcess(string actionName, Action processAction, object? inputData = null)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        Exception? exception = null;

        try
        {
            processAction();
        }
        catch (Exception ex)
        {
            exception = ex;
            throw;
        }
        finally
        {
            stopwatch.Stop();
            Log.ForContext("LogType", LogType.Business)
                .ForContext("Direction", "Process")
                .ForContext("Action", actionName)
                .ForContext("Request", inputData?.ToJson())
                .ForContext("Error", exception)
                .ForContext("Duration", stopwatch.ElapsedMilliseconds)
                .Information("Business Process Log");
        }
    }
}
