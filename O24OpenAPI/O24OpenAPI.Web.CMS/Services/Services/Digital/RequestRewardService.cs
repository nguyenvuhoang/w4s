using LinqToDB;
using O24OpenAPI.Web.CMS.Models.Digital;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.Services;

/// <summary>
/// /// Ctor
/// </summary>
/// <param name="requestrewardRepository"></param>
public class RequestRewardService(IRepository<D_REQUESTREWARD> requestrewardRepository)
    : IRequestRewardService
{
    private readonly IRepository<D_REQUESTREWARD> _requestrewardRepository =
        requestrewardRepository;

    /// <summary>
    /// Gets GetById
    /// </summary>
    /// <param name="id">The id</param>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_REQUESTREWARD> GetByRequestRewardID(int Id)
    {
        return await _requestrewardRepository
            .Table.Where(s => s.Id == Id)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<D_REQUESTREWARD>> GetAll()
    {
        return await _requestrewardRepository.Table.Select(s => s).ToListAsync();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<D_REQUESTREWARD>> Search(SearchRequestRewardModel model)
    {
        var listRequestReward = await (
            from d in _requestrewardRepository.Table
            where
                (
                    (!string.IsNullOrEmpty(model.GiftName) && model.GiftName == d.GiftName)
                    || string.IsNullOrEmpty(model.GiftName)
                )
                && (
                    (!string.IsNullOrEmpty(model.BranchID) && model.BranchID == d.BranchID)
                    || string.IsNullOrEmpty(model.BranchID)
                )
                && (
                    (!string.IsNullOrEmpty(model.Status) && model.Status == d.Status)
                    || string.IsNullOrEmpty(model.Status)
                )
            select d
        ).ToListAsync();

        return listRequestReward.Where(x => x.UserCode == model.CurrentUserCode).ToList();
    }

    /// <summary>
    ///Insert
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_REQUESTREWARD> Insert(D_REQUESTREWARD requestreward)
    {
        var findRequestReward = await _requestrewardRepository
            .Table.Where(s => s.Id.Equals(requestreward.Id))
            .FirstOrDefaultAsync();

        if (findRequestReward != null)
        {
            throw new O24OpenAPIException(
                "InvalidRequestReward",
                "The request reward already existing in system"
            );
        }

        await _requestrewardRepository.Insert(requestreward);

        return requestreward;
    }

    /// <summary>
    ///Update
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_REQUESTREWARD> Update(D_REQUESTREWARD requestreward)
    {
        var findRequestReward = await _requestrewardRepository
            .Table.Where(s => s.Id.Equals(requestreward.Id))
            .FirstOrDefaultAsync();
        if (findRequestReward == null)
        {
            throw new O24OpenAPIException(
                "InvalidCurrency",
                "The currency code does not exist in system"
            );
        }
        else
        {
            findRequestReward.IPCTRANSID = requestreward.IPCTRANSID;
            findRequestReward.UserCode = requestreward.UserCode;
            findRequestReward.Amount = requestreward.Amount;
            findRequestReward.QRCode = requestreward.QRCode;
            findRequestReward.GiftID = requestreward.GiftID;
            findRequestReward.GiftName = requestreward.GiftName;
            findRequestReward.Quantity = requestreward.Quantity;
            findRequestReward.BranchID = requestreward.BranchID;
            findRequestReward.Status = requestreward.Status;
            await _requestrewardRepository.Update(findRequestReward);
        }
        return requestreward;
    }

    /// <summary>
    ///Delete.
    /// </summary>
    /// <returns>Task&lt;Bo&gt;.</returns>
    public virtual async Task<D_REQUESTREWARD> DeleteById(int Id)
    {
        var requestreward = await _requestrewardRepository
            .Table.Where(s => s.Id.Equals(Id))
            .FirstOrDefaultAsync();

        if (requestreward == null)
        {
            throw new O24OpenAPIException(
                "InvalidCurrency",
                "The currency code does not exist in system"
            );
        }
        await _requestrewardRepository.Delete(requestreward);
        return requestreward;
    }
}
