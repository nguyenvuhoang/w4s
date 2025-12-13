using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models.Userlimit;
using O24OpenAPI.Core;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Services.Interfaces;

/// <summary>
/// User limit service interface
/// </summary>
public partial interface IUserLimitService
{
    /// <summary>
    /// get a user limit by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserLimit> GetById(int id);

    /// <summary>
    /// simple search model user limit
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<UserLimit>> Search(SimpleSearchModel model);


    /// <summary>
    /// advanced search user limit
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<UserLimitAdvancedSearchResponseModel>> Search(UserLimitSearchModel model);

    /// <summary>
    /// Create a list of user limits
    /// </summary>
    /// <param name="userLimits"></param>
    /// <returns></returns>
    Task Create(List<UserLimit> userLimits);

    /// <summary>
    /// search a user limit
    /// </summary>
    /// <param name="userLimitId"></param>
    /// <returns></returns>
    Task Delete(int userLimitId);

    /// <summary>
    /// Delete a list of user limits
    /// </summary>
    /// <param name="userLimits"></param>
    /// <returns></returns>
    Task Delete(List<UserLimit> userLimits);

    /// <summary>
    /// get records by role id
    /// </summary>
    /// <returns></returns>
    Task<List<UserLimit>> GetByRoleId(int roleId);

    /// <summary>
    /// Get limit of the user
    /// </summary>
    /// <param name="user"></param>
    /// <param name="commandId"></param>
    /// <param name="currencyCode"></param>
    /// <param name="limitType"></param>
    /// <returns></returns>
    Task<decimal> GetUserLimit(UserAccount user, string commandId, string currencyCode, string limitType = "I");

    /// <summary>
    /// UpdateListUserLimit
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<List<UserLimitUpdateResponseModel>> UpdateListUserLimit(ListUserLimitUpdateModel model);

    /// <summary>
    /// GetUserLimitToUpdate
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<UserLimit> GetUserLimitToUpdate(UserLimitUpdateResponseModel model);
}