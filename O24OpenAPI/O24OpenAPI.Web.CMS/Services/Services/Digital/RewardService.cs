using LinqToDB;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Localization;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class RewardService : IRewardService
{
    private readonly ILocalizationService _localizationService;

    private readonly IRepository<D_REWARDS> _rewardRepository;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="rewardRepository"></param>
    public RewardService(
        ILocalizationService localizationService,
        IRepository<D_REWARDS> rewardRepository
    )
    {
        _localizationService = localizationService;
        _rewardRepository = rewardRepository;
    }

    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <param name="Id">The id</param>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_REWARDS> GetByRewardID(int Id)
    {
        return await _rewardRepository.Table.Where(s => s.Id == Id).FirstOrDefaultAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<D_REWARDS>> GetAll()
    {
        return await _rewardRepository.Table.Select(s => s).ToListAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<D_REWARDS>> Search(SearchRewardModel model)
    {
        var listReward = await (
            from d in _rewardRepository.Table
            where
                (
                    (!string.IsNullOrEmpty(model.GiftName) && model.GiftName == d.GiftName)
                    || string.IsNullOrEmpty(model.GiftName)
                )
                && (
                    (!string.IsNullOrEmpty(model.Status) && model.Status == d.Status)
                    || string.IsNullOrEmpty(model.Status)
                )
            select d
        ).ToListAsync();

        return listReward;
    }

    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_REWARDS> Insert(D_REWARDS reward)
    {
        var findReward = await _rewardRepository
            .Table.Where(s => s.Id.Equals(reward.Id))
            .FirstOrDefaultAsync();
        if (findReward == null)
        {
            await _rewardRepository.Insert(reward);
        }
        else
        {
            throw new O24OpenAPIException(
                "InvalidReward",
                "The reward code already existing in system"
            );
        }

        return reward;
    }

    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_REWARDS> Update(D_REWARDS reward)
    {
        var findReward = await _rewardRepository
            .Table.Where(s => s.Id.Equals(reward.Id))
            .FirstOrDefaultAsync();
        if (findReward == null)
        {
            throw new O24OpenAPIException(
                "InvalidReward",
                "The reward code does not exist in system"
            );
        }
        else
        {
            findReward.GiftName = reward.GiftName;
            findReward.LocalGiftName = reward.LocalGiftName;
            findReward.Type = reward.Type;
            findReward.BranchID = reward.BranchID;
            findReward.RequiredPoints = reward.RequiredPoints;
            findReward.LimitGiftPerRedeem = reward.LimitGiftPerRedeem;
            findReward.QuantityGift = reward.QuantityGift;
            findReward.Status = reward.Status;
            await _rewardRepository.Update(findReward);
        }
        return reward;
    }

    /// <summary>
    ///Delete.
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_REWARDS> DeleteById(int Id)
    {
        var reward = await _rewardRepository
            .Table.Where(s => s.Id.Equals(Id))
            .FirstOrDefaultAsync();

        if (reward == null)
        {
            throw new O24OpenAPIException(
                "InvalidReward",
                "The reward code does not exist in system"
            );
        }
        await _rewardRepository.Delete(reward);
        return reward;
    }
}
