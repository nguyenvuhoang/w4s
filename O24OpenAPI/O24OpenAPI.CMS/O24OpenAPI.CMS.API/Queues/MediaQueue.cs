using O24OpenAPI.CMS.API.Application.Models.Media;
using O24OpenAPI.CMS.API.Application.Services.Interfaces.Media;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Services.Queue;

namespace O24OpenAPI.CMS.API.Queues;

public class MediaQueue : BaseQueue
{
    private readonly IMediaService _mediaService = EngineContext.Current.Resolve<IMediaService>();

    public async Task<WFScheme> SyncMedia(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<MediaSyncModel>();
        return await Invoke<MediaSyncModel>(
            wFScheme,
            async () =>
            {
                var response = await _mediaService.SyncMediaAsync(model);
                return response;
            }
        );
    }
}
