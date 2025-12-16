using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Services.Queue;
using O24OpenAPI.Web.CMS.Models.Media;
using O24OpenAPI.Web.CMS.Services.Interfaces.Media;

namespace O24OpenAPI.Web.CMS.Queues;

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
