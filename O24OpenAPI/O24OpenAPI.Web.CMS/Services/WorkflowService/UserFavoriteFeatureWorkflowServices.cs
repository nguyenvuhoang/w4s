using O24OpenAPI.Web.CMS.Services.Interfaces.Digital;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class UserFavoriteFeatureWorkflowServices : BaseQueueService
{
    private readonly IUserFavoriteFeatureService _contextIUserFavoriteFeature =
        EngineContext.Current.Resolve<IUserFavoriteFeatureService>();

    public async Task<WorkflowScheme> GetAll(WorkflowScheme workflow)
    {
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var list = await _contextIUserFavoriteFeature.GetAll();
                return list;
            }
        );
    }

    public async Task<WorkflowScheme> Create(WorkflowScheme workflow)
    {
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var list = await _contextIUserFavoriteFeature.GetAll();
                return list;
            }
        );
    }
}
