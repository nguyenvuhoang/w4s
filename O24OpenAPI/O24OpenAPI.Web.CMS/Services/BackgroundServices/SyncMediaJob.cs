using LinKit.Core.BackgroundJobs;
using LinKit.Core.Cqrs;
using O24OpenAPI.Web.CMS.Services.Interfaces.Media;
using O24OpenAPI.Web.Framework.Extensions;

namespace O24OpenAPI.Web.CMS.Services.BackgroundServices;

[BackgroundJob("SyncMediaJob")]
public class SyncMediaJob : BackgroundJobCommand { }

[CqrsHandler]
internal class SyncMediaJobHandler(IMediaService mediaService)
    : ICommandHandler<SyncMediaJob>
{
    private readonly IMediaService _mediaService = mediaService;

    public async Task HandleAsync(
        SyncMediaJob command,
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
            Console.WriteLine("SyncMediaJob Starting::" + localTime);
            Console.ForegroundColor = ConsoleColor.White;
            await _mediaService.PromoteMedia();
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync("SyncMediaJob Exception:: " + ex.Message);
        }
    }
}
