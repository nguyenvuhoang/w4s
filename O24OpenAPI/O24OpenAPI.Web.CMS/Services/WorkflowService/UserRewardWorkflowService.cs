using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class UserRewardWorkflowService : BaseQueueService
{
    private readonly IUserRewardService _userRewardService = EngineContext.Current.Resolve<IUserRewardService>();

    public async Task<WorkflowScheme> GetPointOfUser(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<BaseTransactionModel>();

        return await Invoke<BaseTransactionModel>(workflow, async ()=>
        {
            var totalPoint = await _userRewardService.GetTotalPoint(model.CurrentUserCode);
            return totalPoint;
        });
    }
}
