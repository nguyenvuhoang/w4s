using LinKit.Core.BackgroundJobs;
using LinKit.Core.Cqrs;
using O24OpenAPI.O24ACT.Services.Interfaces;
using O24OpenAPI.Framework.Extensions;

namespace O24OpenAPI.O24ACT.Services.BackgroundServices;

[BackgroundJob("SyncGLEntriesJob")]
public class SyncGLEntriesJob : BackgroundJobCommand { }

[CqrsHandler]
internal class SyncGLEntriesJobHandler(IAccountingService accountingService)
    : ICommandHandler<SyncGLEntriesJob>
{
    private readonly IAccountingService _accountingService = accountingService;

    public async Task<Unit> HandleAsync(
        SyncGLEntriesJob command,
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
            Console.WriteLine("SyncGLEntries Starting::" + localTime);
            Console.ForegroundColor = ConsoleColor.White;
            await _accountingService.SyncGLEntries();
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync("SyncGLEntriesJob Exception:: " + ex.Message);
        }

        return Unit.Value;
    }
}
