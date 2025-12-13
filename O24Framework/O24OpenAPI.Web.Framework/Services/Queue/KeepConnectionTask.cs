using Microsoft.Extensions.DependencyInjection;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.O24OpenAPIClient;
using O24OpenAPI.Web.Framework.Services.Logging;
using O24OpenAPI.Web.Framework.Services.ScheduleTasks;

namespace O24OpenAPI.Web.Framework.Services.Queue;

/// <summary>
/// The keep connection task class
/// </summary>
/// <seealso cref="IScheduleTask"/>
public class KeepConnectionTask : IScheduleTask
{
    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="KeepConnectionTask"/> class
    /// </summary>
    /// <param name="logger">The logger</param>
    public KeepConnectionTask(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Executes a task
    /// </summary>
    public async Task Execute(DateTime? lastSuccess, IServiceScope serviceScope)
    {
        try
        {
            QueueContext.GetInstance();
        }
        catch (Exception ex)
        {
            await _logger.Error("Cannot create queue client connect to portal", ex);
            Singleton<QueueClient>.Instance = null;
        }
    }
}
