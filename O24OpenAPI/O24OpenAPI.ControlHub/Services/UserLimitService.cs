using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models.Userlimit;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Core.SeedWork;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Web.Framework.Localization;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Services;

/// <summary>
/// User limit services
/// </summary>
/// <remarks>
/// User limit service constructor
/// </remarks>
/// <param name="userLimitRepository"></param>
/// <param name="roleUserRepository"></param>
/// <param name="localizationService"></param>
/// <param name="userCommandRepository"></param>
/// <param name="staticCacheManager"></param>
public partial class UserLimitService(
    IRepository<UserLimit> userLimitRepository,
    IRepository<UserInRole> roleUserRepository,
    ILocalizationService localizationService,
    IRepository<UserCommand> userCommandRepository,
    IStaticCacheManager staticCacheManager
) : IUserLimitService
{
    #region  Fields
    private readonly StringComparison ICIC = StringComparison.InvariantCultureIgnoreCase;
    private readonly IRepository<UserLimit> _userLimitRepository = userLimitRepository;
    private readonly IRepository<UserInRole> _roleUserRepository = roleUserRepository;
    private readonly IRepository<UserCommand> _userCommandRepository = userCommandRepository;
    private readonly ILocalizationService _localizationService = localizationService;
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;

    #endregion
    #region Ctor

    #endregion

    /// <summary>
    /// Get a user limit by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<UserLimit> GetById(int id)
    {
        return await _userLimitRepository.GetById(id, cache => default);
    }

    /// <summary>
    /// Simple search model user limit
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<UserLimit>> Search(SimpleSearchModel model)
    {
        model.PageSize = model.PageSize == 0 ? int.MaxValue : model.PageSize;
        if (model.SearchText == null)
        {
            model.SearchText = String.Empty;
        }

        var userLimits = await _userLimitRepository.GetAllPaged(
            query =>
            {
                query = query.Where(c =>
                    c.RoleId.ToString().Contains(model.SearchText, ICIC)
                    || c.CommandId.Contains(model.SearchText, ICIC)
                );
                query = query.OrderByDescending(a => a.Id);
                return query;
            },
            model.PageIndex,
            model.PageSize
        );
        return userLimits;
    }

    /// <summary>
    /// Advanced search user limit
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<UserLimitAdvancedSearchResponseModel>> Search(
        UserLimitSearchModel model
    )
    {
        model.PageSize = model.PageSize == 0 ? int.MaxValue : model.PageSize;

        var uLimit = _userLimitRepository.Table;
        if (model.RoleId != null)
        {
            uLimit = uLimit.Where(x => x.RoleId == model.RoleId);
        }

        if (model.CommandId != null)
        {
            uLimit = uLimit.Where(x => x.CommandId.Contains(model.CommandId, ICIC));
        }

        if (model.LimitType.HasValue())
        {
            uLimit = uLimit.Where(x => x.LimitType.Equals(model.LimitType));
        }

        uLimit.OrderBy(o => o.Id);

        var result = await (
            from a in uLimit
            join userCommand1 in _userCommandRepository.Table.Where(s =>
                s.CommandType == "T" && s.Enabled == true
            )
                on a.CommandId equals userCommand1.CommandId
                into j1
            from b in j1 //.DefaultIfEmpty()
            join userCommand2 in _userCommandRepository.Table.Where(s =>
                s.CommandType == "T" && s.Enabled == true
            )
                on b.ParentId equals userCommand2.CommandId
                into j2
            from c in j2.DefaultIfEmpty()
            select new UserLimitAdvancedSearchResponseModel()
            {
                Id = a.Id,
                RoleId = a.RoleId,
                CommandId = a.CommandId,
                CurrencyCode = a.CurrencyCode,
                ULimit = a.ULimit,
                CvTable = a.CvTable,
                LimitType = a.LimitType,
                Margin = a.Margin,
                Module = c.CommandName,
                TranName = b.CommandName,
            }
        )
            .OrderBy(o => o.Module)
            .ThenBy(o => o.TranName)
            .ToPagedList(model.PageIndex, model.PageSize);
        return result;
    }

    /// <summary>
    /// Create a list of user limits
    /// </summary>
    /// <param name="userLimits"></param>
    /// <returns></returns>
    public virtual async Task Create(List<UserLimit> userLimits)
    {
        await _userLimitRepository.BulkInsert(userLimits);
    }

    /// <summary>
    /// Delete a user limit
    /// </summary>
    /// <param name="userLimitId"></param>
    /// <returns></returns>
    public virtual async Task Delete(int userLimitId)
    {
        var userLimit =
            await _userLimitRepository.GetById(userLimitId)
            ?? throw new O24OpenAPIException(
                await _localizationService.GetResource("Admin.UserLimit.Value.NotFound")
            );
        await _userLimitRepository.Delete(userLimit);
    }

    /// <summary>
    /// Delete a list of user limits
    /// </summary>
    /// <param name="userLimits"></param>
    /// <returns></returns>
    public virtual async Task Delete(List<UserLimit> userLimits)
    {
        if (userLimits.Count > 0)
        {
            await _userLimitRepository.BulkDelete(userLimits);
        }
    }

    /// <summary>
    /// get records by role id
    /// </summary>
    /// <returns></returns>
    public async Task<List<UserLimit>> GetByRoleId(int roleId)
    {
        return await _userLimitRepository.Table.Where(uL => uL.RoleId == roleId).ToListAsync();
    }

    /// <summary>
    /// Get limit of the user
    /// </summary>
    /// <param name="user"></param>
    /// <param name="commandId"></param>
    /// <param name="currencyCode"></param>
    /// <param name="limitType"></param>
    /// <returns></returns>
    public async Task<decimal> GetUserLimit(
        UserAccount user,
        string commandId,
        string currencyCode,
        string limitType = "I"
    )
    {
        var query =
            from l in _userLimitRepository.Table
            join r in _roleUserRepository.Table on l.RoleId equals r.RoleId
            where
                r.UserCode == user.UserCode && l.CommandId == commandId && l.LimitType == limitType
            select l;
        var limits = await query.ToListAsync();
        if (limits.Any())
        {
            //Add condition
            if (limits.All(s => s.ULimit == null))
            {
                throw new Exception("Unauthorized");
            }

            var limit = limits.Max(l => l.ULimit) ?? 0;
            return limit;
        }
        return 0;
    }

    /// <summary>
    /// UpdateListUserLimit
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<List<UserLimitUpdateResponseModel>> UpdateListUserLimit(
        ListUserLimitUpdateModel model
    )
    {
        UserLimit checkUserLimit;
        var respone = new List<UserLimitUpdateResponseModel>();

        foreach (var item in model.ListUserLimit)
        {
            checkUserLimit = await GetUserLimitToUpdate(item);

            if (checkUserLimit != null)
            {
                checkUserLimit.ULimit = item.ULimit;
                await _userLimitRepository.Update(checkUserLimit);
            }
            else
            {
                checkUserLimit = new UserLimit()
                {
                    RoleId = item.RoleId,
                    CommandId = item.CommandId,
                    CurrencyCode = item.CurrencyCode,
                    ULimit = item.ULimit,
                    CvTable = string.Empty,
                    LimitType = item.LimitType,
                    Margin = 0,
                };
                await _userLimitRepository.InsertAsync(checkUserLimit);
            }
            respone.Add(
                new UserLimitUpdateResponseModel()
                {
                    RoleId = checkUserLimit.RoleId,
                    CommandId = checkUserLimit.CommandId,
                    CurrencyCode = checkUserLimit.CurrencyCode,
                    ULimit = checkUserLimit.ULimit,
                    LimitType = checkUserLimit.LimitType,
                }
            );
        }
        return respone;
    }

    /// <summary>
    /// GetUserLimitToUpdate
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<UserLimit> GetUserLimitToUpdate(UserLimitUpdateResponseModel model)
    {
        var uLimit = await _userLimitRepository
            .Table.Where(s =>
                s.RoleId == model.RoleId
                && s.CommandId == model.CommandId
                && s.CurrencyCode == model.CurrencyCode
                && s.LimitType == model.LimitType
            )
            .FirstOrDefaultAsync();
        return uLimit;
    }
}
