using LinqToDB;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;
using O24OpenAPI.Web.Framework.Localization;

namespace O24OpenAPI.Web.CMS.Services.Services;

public class UserRewardService : IUserRewardService
{
    private readonly ILocalizationService _localizationService;

    private readonly IRepository<D_USER_REWARD> _userrewardRepository;

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="localizationService"></param>
    /// <param name="rewardRepository"></param>
    public UserRewardService(
        ILocalizationService localizationService,
        IRepository<D_USER_REWARD> userrewardRepository
    )
    {
        _localizationService = localizationService;
        _userrewardRepository = userrewardRepository;
    }

    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <param name="Id">The id</param>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_USER_REWARD> GetByUserRewardID(int Id)
    {
        return await _userrewardRepository.Table.Where(s => s.Id == Id).FirstOrDefaultAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<D_USER_REWARD>> GetAll()
    {
        return await _userrewardRepository.Table.Select(s => s).ToListAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<D_USER_REWARD>> Search(SearchUserRewardModel model)
    {
        var listUserReward = await (
            from d in _userrewardRepository.Table
            where
                (!string.IsNullOrEmpty(model.UserCode) && model.UserCode == d.UserCode)
                || string.IsNullOrEmpty(model.UserCode)

            select d
        ).ToListAsync();

        return listUserReward;
    }

    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_USER_REWARD> Insert(D_USER_REWARD userreward)
    {
        var findUserReward = await _userrewardRepository
            .Table.Where(s => s.Id.Equals(userreward.Id))
            .FirstOrDefaultAsync();
        if (findUserReward == null)
        {
            await _userrewardRepository.Insert(userreward);
        }
        else
        {
            throw new O24OpenAPIException(
                "InvalidUserReward",
                "The user reward code already existing in system"
            );
        }

        return userreward;
    }

    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_USER_REWARD> Update(D_USER_REWARD userreward)
    {
        var findUserReward = await _userrewardRepository
            .Table.Where(s => s.Id.Equals(userreward.Id))
            .FirstOrDefaultAsync();
        if (findUserReward == null)
        {
            throw new O24OpenAPIException(
                "InvalidUserReward",
                "The User reward code does not exist in system"
            );
        }
        else
        {
            findUserReward.UserCode = userreward.UserCode;
            findUserReward.TotalPoint = userreward.TotalPoint;
            findUserReward.UsedPoint = userreward.UsedPoint;
            findUserReward.GiftId = userreward.GiftId;
            findUserReward.EventId = userreward.EventId;
            findUserReward.IssueDate = userreward.IssueDate;
            findUserReward.ExpiryDate = userreward.ExpiryDate;
            findUserReward.Descriptions = userreward.Descriptions;
            await _userrewardRepository.Update(findUserReward);
        }
        return userreward;
    }

    /// <summary>
    ///Delete.
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_USER_REWARD> DeleteById(int Id)
    {
        var reward = await _userrewardRepository
            .Table.Where(s => s.Id.Equals(Id))
            .FirstOrDefaultAsync();

        if (reward == null)
        {
            throw new O24OpenAPIException(
                "InvalidReward",
                "The reward code does not exist in system"
            );
        }
        await _userrewardRepository.Delete(reward);
        return reward;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<decimal> GetTotalPoint(string usercode)
    {
        var currentDate = DateTime.Now.Date;
        var sumPoint = _userrewardRepository
            .Table.Where(x => x.UserCode == usercode && x.ExpiryDate >= currentDate)
            .Select(x => x.TotalPoint - x.UsedPoint)
            .Sum();

        return await Task.FromResult(sumPoint);
    }
}
