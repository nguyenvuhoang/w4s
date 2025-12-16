using O24OpenAPI.O24NCH.Services.Interfaces;
using O24OpenAPI.Framework.Services.ScheduleTasks;

namespace O24OpenAPI.O24NCH.Services.ScheduleTasks;

public class SyncSMSProviderStatus : IScheduleTask
{
    public async Task Execute(DateTime? lastSuccess, IServiceScope serviceScope)
    {
        Console.WriteLine("SyncSMSProviderStatus lastSuccess::" + lastSuccess);
        Console.WriteLine("serviceScope:: " + serviceScope.GetHashCode());
        var providerService = serviceScope.ServiceProvider.GetService<ISMSProviderService>();
        await providerService.SyncSMSProviderStatusAsync();
    }
}
