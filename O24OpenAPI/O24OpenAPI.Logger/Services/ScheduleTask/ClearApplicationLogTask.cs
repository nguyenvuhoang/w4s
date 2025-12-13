using O24OpenAPI.Logger.Services.Interfaces;
using O24OpenAPI.Web.Framework.Services.ScheduleTasks;

namespace O24OpenAPI.Logger.Services.ScheduleTask;

public class ClearApplicationLogTask : IScheduleTask
{
    public async Task Execute(DateTime? lastSuccess, IServiceScope serviceScope)
    {
        var serviceProvider = serviceScope.ServiceProvider;
        var applicationLogService = serviceProvider.GetService<IApplicationLogService>();
        await applicationLogService.ClearApplicationLogAsync();
    }
}
