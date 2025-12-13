using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Models.O24OpenAPI;
using O24OpenAPI.Web.Framework.Services.Queue;

namespace O24OpenAPI.Web.CMS.Queues;

public class AppLanguageConfigQueue : BaseQueue
{
    private readonly IAppLanguageConfigService _appLanguageService =
        EngineContext.Current.Resolve<IAppLanguageConfigService>();

    /// <summary>
    /// Load cấu hình ngôn ngữ (auto chọn giữa LoadAppLanguageAsync hoặc LoadAppLanguagePageAsync)
    /// </summary>
    public async Task<WFScheme> LoadAppLanguageConfig(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<AppLanguageConfigRequestModel>();

        return await Invoke<AppLanguageConfigRequestModel>(
            wFScheme,
            async () =>
            {
                bool doPaging = model.PageIndex.HasValue && model.PageSize.HasValue &&
                                model.PageIndex.Value >= 0 && model.PageSize.Value > 0;

                if (doPaging)
                {
                    var paged = await _appLanguageService.LoadAppLanguagePageAsync(model);
                    return paged;
                }

                var simpleModel = new AppLanguageConfigRequestModel
                {
                    ChannelId = model.ChannelId,
                    RequestChannel = model.RequestChannel
                };

                var full = await _appLanguageService.LoadAppLanguageAsync(simpleModel);
                return full;
            }
        );
    }
}
