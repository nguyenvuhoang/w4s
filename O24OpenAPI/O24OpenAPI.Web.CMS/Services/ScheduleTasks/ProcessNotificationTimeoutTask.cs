using O24OpenAPI.Web.CMS.Services.Interfaces.Digital;
using O24OpenAPI.Framework.Services.ScheduleTasks;

namespace O24OpenAPI.Web.CMS.Services.ScheduleTasks;

public class ProcessNotificationTimeoutTask : IScheduleTask
{
    public async Task Execute(DateTime? lastSuccess, IServiceScope serviceScope)
    {
        Console.WriteLine("ProcessNotificationTimeoutTask lastSuccess::" + lastSuccess);
        Console.WriteLine("serviceScope:: " + serviceScope.GetHashCode());
        var notificationService =
            serviceScope.ServiceProvider.GetService<INotificationService>();
        await notificationService.ProcessNotificationTimeout();
    }
}
