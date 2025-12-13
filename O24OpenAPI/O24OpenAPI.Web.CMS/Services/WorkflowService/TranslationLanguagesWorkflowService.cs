using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class TranslationLanguagesWorkflowService : BaseQueueService
{
    private readonly IAppLanguageConfigService _appLanguageService = EngineContext.Current.Resolve<IAppLanguageConfigService>();
    public async Task<WorkflowScheme> LoadAppLanguageVersion(WorkflowScheme wFScheme)
    {
        var model = await wFScheme.ToModel<AppLanguageConfigRequestModel>();

        return await Invoke<BaseTransactionModel>(wFScheme, async () =>
        {
            var simpleModel = new AppLanguageConfigRequestModel
            {
                ChannelId = model.ChannelId,
                RequestChannel = model.RequestChannel
            };

            var version = await _appLanguageService.LoadAppLanguageVersionAsync(simpleModel);
            return version;
        });
    }
}
