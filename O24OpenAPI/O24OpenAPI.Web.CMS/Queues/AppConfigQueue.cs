using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Models.O24OpenAPI;
using O24OpenAPI.Web.Framework.Services.Queue;

namespace O24OpenAPI.Web.CMS.Queues;

public class AppTypeConfigQueue : BaseQueue
{
    private readonly IAppTypeConfigService _loadAppConfigTypeService = EngineContext.Current.Resolve<IAppTypeConfigService>();

    public async Task<WFScheme> LoadAppTypeConfig(WFScheme wFScheme)
    {
        var model = await wFScheme.ToModel<AppTypeConfigRequestModel>();
        return await Invoke<AppTypeConfigRequestModel>(
            wFScheme,
            async () =>
            {
                var response = await _loadAppConfigTypeService.LoadAppTypeConfig(model);
                return response;
            }
        );
    }
}
