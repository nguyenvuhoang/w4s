using O24OpenAPI.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class RewardWorkflowService : BaseQueueService
{
    private readonly IRewardService _rewardService =
        EngineContext.Current.Resolve<IRewardService>();

    public async Task<WorkflowScheme> GetAll(WorkflowScheme workflow)
    {
        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var list = await _rewardService.GetAll();
                return list;
            }
        );
    }

    public async Task<WorkflowScheme> GetByRewardID(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<GetRewardByIDModel>();

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var reward = await _rewardService.GetByRewardID(model.Id);
                return reward;
            }
        );
    }

    public async Task<WorkflowScheme> Insert(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<CurrencyModel>();

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var newReward = new D_REWARDS();
                newReward = model.ToEntity(newReward);
                var reward = await _rewardService.Insert(newReward);
                return reward;
            }
        );
    }

    public async Task<WorkflowScheme> Update(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<RewardModel>();

        if (string.IsNullOrEmpty(model.GiftName))
        {
            throw new O24OpenAPIException("InvalidReward", "The Reward is required");
        }

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var newReward = new D_REWARDS();
                newReward = model.ToEntity(newReward);
                var reward = await _rewardService.Update(newReward);
                return reward;
            }
        );
    }

    public async Task<WorkflowScheme> DeleteById(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<GetRewardByIDModel>();

        return await Invoke<BaseTransactionModel>(
            workflow,
            async () =>
            {
                var reward = await _rewardService.DeleteById(model.Id);
                return reward;
            }
        );
    }

    public async Task<WorkflowScheme> Search(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<SearchRewardModel>();

        return await Invoke<SearchRewardModel>(
            workflow,
            async () =>
            {
                var list = await _rewardService.Search(model);
                return list;
            }
        );
    }
}
