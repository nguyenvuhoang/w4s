using LinKit.Core.BackgroundJobs;
using LinKit.Core.Cqrs;
using O24OpenAPI.O24NCH.Services.Interfaces;
using O24OpenAPI.Web.Framework.Extensions;

namespace O24OpenAPI.O24NCH.Services.BackgroundServices;

[BackgroundJob("ScanUserNotificationJob")]
public class ScanUserNotificationJob : BackgroundJobCommand { }

[CqrsHandler]
public class ScanUserNotificationJobHandler(IUserNotificationsService userNotificationsService)
    : ICommandHandler<ScanUserNotificationJob>
{
    private readonly IUserNotificationsService _userNotificationsService = userNotificationsService;


    public async Task HandleAsync(
        ScanUserNotificationJob command,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(
                DateTime.UtcNow,
                TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
            );
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[DTS] ScanUserNotificationJob Starting::" + localTime);
            Console.ForegroundColor = ConsoleColor.White;
            await _userNotificationsService.ScanUserNotification(cancellationToken);
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync("ScanUserNotificationJob Exception:: " + ex.Message);
        }
    }
}
