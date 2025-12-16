using O24OpenAPI.Framework.Services.ScheduleTasks;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.ScheduleTasks;

public class BackupTask : IScheduleTask
{
    public async Task Execute(DateTime? lastSuccess, IServiceScope serviceScope)
    {
        Console.WriteLine("BackupTask Execute :: " + DateTime.Now.ToString());
        Console.WriteLine("serviceScope:: " + serviceScope.GetHashCode());
        var service = serviceScope.ServiceProvider.GetService<IDataService>();
        await service.ExportAllData("");
    }
}
