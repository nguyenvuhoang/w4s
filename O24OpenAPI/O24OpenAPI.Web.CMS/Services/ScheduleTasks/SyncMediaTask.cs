using O24OpenAPI.Web.CMS.Services.Interfaces.Media;
using O24OpenAPI.Framework.Services.ScheduleTasks;

namespace O24OpenAPI.Web.CMS.Services.ScheduleTasks;

public class SyncMediaTask : IScheduleTask
{
    public async Task Execute(DateTime? lastSuccess, IServiceScope serviceScope)
    {
        Console.WriteLine("SyncMediaTask lastSuccess::" + lastSuccess);
        Console.WriteLine("serviceScope:: " + serviceScope.GetHashCode());
        var mediaService = serviceScope.ServiceProvider.GetService<IMediaService>();
        await mediaService.PromoteMedia();
    }
}
