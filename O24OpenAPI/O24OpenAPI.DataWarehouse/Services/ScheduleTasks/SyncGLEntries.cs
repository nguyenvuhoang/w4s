using O24OpenAPI.DataWarehouse.Services.Interface;
using O24OpenAPI.Framework.Services.ScheduleTasks;

namespace O24OpenAPI.DataWarehouse.Services.ScheduleTasks;

public class SyncGLEntries : IScheduleTask
{
    public async Task Execute(DateTime? lastSuccess, IServiceScope serviceScope)
    {
        Console.WriteLine("SyncGLEntries lastSuccess::" + lastSuccess);
        Console.WriteLine("serviceScope:: " + serviceScope.GetHashCode());
        var smsAutoBalanceService = serviceScope.ServiceProvider.GetService<IAccountingService>();
        await smsAutoBalanceService.SyncGLEntries();
    }
}
