using LinKit.Core.BackgroundJobs;
using LinKit.Core.Cqrs;
using O24OpenAPI.Framework.Extensions;

namespace O24OpenAPI.NCH.API.Application.BackgroundServices;

[BackgroundJob("SubmitLoanAlertJob")]
public class SubmitLoanAlertJob : BackgroundJobCommand { }

[CqrsHandler]
public class SubmitLoanAlertJobHandler : ICommandHandler<SubmitLoanAlertJob>
{
    public async Task<Unit> HandleAsync(
        SubmitLoanAlertJob request,
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
            Console.WriteLine("[DTS] SubmitLoanAlertJob Starting::" + localTime);
            Console.ForegroundColor = ConsoleColor.White;

            // NOTE: bản rút gọn theo CTH - chưa port logic gửi alert.
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync("SubmitLoanAlertJob Exception:: " + ex.Message);
        }

        return Unit.Value;
    }
}
