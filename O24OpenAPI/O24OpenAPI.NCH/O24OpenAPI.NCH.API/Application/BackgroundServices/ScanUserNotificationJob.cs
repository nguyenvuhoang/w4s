using LinKit.Core.BackgroundJobs;
using LinKit.Core.Cqrs;
using O24OpenAPI.Framework.Extensions;

namespace O24OpenAPI.NCH.API.Application.BackgroundServices;

[BackgroundJob("ScanUserNotificationJob")]
public class ScanUserNotificationJob : BackgroundJobCommand { }

[CqrsHandler]
public class ScanUserNotificationJobHandler : ICommandHandler<ScanUserNotificationJob>
{
    public async Task<Unit> HandleAsync(
        ScanUserNotificationJob request,
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

            // NOTE:
            // - Bản rút gọn theo CTH: tạm thời không chạy business logic (vì service layer O24NCH đã bỏ).
            // - Khi cần, mình sẽ tạo Application handler/use-case dùng Repository trong NCH.Infrastructure.
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync("ScanUserNotificationJob Exception:: " + ex.Message);
        }

        return Unit.Value;
    }
}
