using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Extensions;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Web.Framework.Services;

namespace O24OpenAPI.Web.CMS.Services.WorkflowService;

public class RequestRewardWorkflowService : BaseQueueService
{
    private readonly IRequestRewardService _requestrewardService = EngineContext.Current.Resolve<IRequestRewardService>();
    private readonly IRepository<D_USER_REWARD> _userrewardRepository = EngineContext.Current.Resolve<IRepository<D_USER_REWARD>>();
    private readonly IRepository<D_REWARDS> _rewardRepository = EngineContext.Current.Resolve<IRepository<D_REWARDS>>();
    public async Task<WorkflowScheme> GetAll(WorkflowScheme workflow)
    {
        return await Invoke<BaseTransactionModel>(workflow, async () =>
        {
            var list = await _requestrewardService.GetAll();
            return list;
        });
    }
    public async Task<WorkflowScheme> GetByRequestRewardID(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<GetRequestRewardByIDModel>();

        return await Invoke<BaseTransactionModel>(workflow, async () =>
        {
            var requestreward = await _requestrewardService.GetByRequestRewardID(model.Id);
            return requestreward;
        });
    }
    public async Task<WorkflowScheme> Insert(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<RequestRewardModel>();

        return await Invoke<BaseTransactionModel>(workflow, async () =>
        {
            var newRequestReward = new D_REQUESTREWARD();
            newRequestReward = model.ToEntity(newRequestReward);
            newRequestReward.UserCode = model.CurrentUserCode;

            var reward = _rewardRepository.Table.Where(r => r.Id == newRequestReward.GiftID).FirstOrDefault();

            if (reward == null)
            {
                throw new O24OpenAPIException("InvalidReward", "The specified reward does not exist.");
            }

            var userRewards = await _userrewardRepository.Table.Where(x => x.UserCode == model.CurrentUserCode).Select(t => t.TotalPoint - t.UsedPoint).ToListAsync();

            var totalPoint = userRewards.Sum();

            if (totalPoint < reward.RequiredPoints)
            {
                throw new O24OpenAPIException("InsufficientPoints", "Not enough points to request the reward.");
            }

            var requestreward = await _requestrewardService.Insert(newRequestReward);

            return requestreward;
        });
    }
    public async Task<WorkflowScheme> Update(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<RequestRewardModel>();

        if (string.IsNullOrEmpty(model.IPCTRANSID))
        {
            throw new O24OpenAPIException("InvalidIPCTRANSID", "The IPCTRANSID  is required");
        }

        return await Invoke<BaseTransactionModel>(workflow, async () =>
        {
            var newRequestReward = new D_REQUESTREWARD();
            newRequestReward = model.ToEntity(newRequestReward);
            var requestreward = await _requestrewardService.Update(newRequestReward);
            return requestreward;
        });
    }
    public async Task<WorkflowScheme> DeleteById(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<GetRequestRewardByIDModel>();

        return await Invoke<BaseTransactionModel>(workflow, async () =>
        {
            var requestreward = await _requestrewardService.DeleteById(model.Id);
            return requestreward;
        });
    }
    public async Task<WorkflowScheme> Search(WorkflowScheme workflow)
    {
        var model = await workflow.ToModel<SearchRequestRewardModel>();

        return await Invoke<SearchRequestRewardModel>(workflow, async () =>
        {
            var list = await _requestrewardService.Search(model);
            return list;
        });
    }
}
