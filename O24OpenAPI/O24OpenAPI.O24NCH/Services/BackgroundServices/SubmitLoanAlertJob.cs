using LinKit.Core.BackgroundJobs;
using LinKit.Core.Cqrs;
using O24OpenAPI.O24NCH.Services.Interfaces;
using O24OpenAPI.Web.Framework.Extensions;

namespace O24OpenAPI.O24NCH.Services.BackgroundServices;

[BackgroundJob("SubmitLoanAlertJob")]
public class SubmitLoanAlertJob : BackgroundJobCommand { }

[CqrsHandler]
internal class SubmitLoanAlertJobHandler(ISMSLoanAlertService smsLoanAlert)
    : ICommandHandler<SubmitLoanAlertJob>
{
    private readonly ISMSLoanAlertService _smsLoanAlert = smsLoanAlert;

    public async Task HandleAsync(
        SubmitLoanAlertJob command,
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
            await _smsLoanAlert.SubmitSMSLoanAlert(ct: cancellationToken);
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync("SubmitLoanAlertJob Exception:: " + ex.Message);
        }
    }
}
