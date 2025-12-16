using O24OpenAPI.Core;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.O24ACT.Models;

namespace O24OpenAPI.O24ACT.Services;

/// <summary>
/// IAccountCommonService
/// </summary>
public partial interface IAccountCommonService
{
    /// <summary>
    /// GetById
    /// </summary>
    /// <param name="commonId"></param>
    /// <returns></returns>
    Task<AccountCommon> GetById(int commonId);

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<AccountCommon>> Search(SimpleSearchModel model);

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<AccountCommon>> Search(AccountCommonSearchModel model);

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="common"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task<AccountCommon> Create(AccountCommon common, string referenceId = "");

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="common"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task Update(AccountCommon common, string referenceId = "");

    /// <summary>
    /// Check Account Name Exist
    /// </summary>
    /// <param name="acountName"></param>
    /// <returns></returns>
    bool IsAccountNameExist(string acountName);

    /// <summary>
    /// Get By Account Name
    /// </summary>
    /// <param name="acountName"></param>
    /// <returns></returns>
    Task<AccountCommon> GetByAccountName(string acountName);

    /// <summary>
    ///
    /// </summary>
    /// <param name="accountName"></param>
    /// <param name="branchCode"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    Task<string> GetAccountNumber(string accountName, string branchCode, string currencyCode);
}
