using O24OpenAPI.Core;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.O24ACT.Models;

namespace O24OpenAPI.O24ACT.Services.Interfaces;

public interface IAccountClearingService
{
    /// <summary>
    /// GetById
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<AccountClearing> GetById(int id);

    /// <summary>
    /// GetByUniqueKey
    /// </summary>
    /// <param name="branchCode"></param>
    /// <param name="currencyId"></param>
    /// <param name="clearingBranchCode"></param>
    /// <param name="clearingType"></param>
    /// <returns></returns>
    Task<AccountClearing> GetByUniqueKey(
        string branchCode,
        string currencyId,
        string clearingBranchCode,
        string clearingType
    );

    /// <summary>
    /// View
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<AccountClearingViewReponseModel> View(int id);

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<AccountClearingSearchReponseModel>> Search(SimpleSearchModel model);

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<AccountClearingSearchReponseModel>> Search(AccountClearingSearchModel model);

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="clearing"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task<AccountClearingCRUDReponseModel> Create(AccountClearing clearing, string referenceId = "");

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="clearing"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task Update(AccountClearing clearing, string referenceId = "");

    /// <summary>
    /// IsUniqueKeyExist
    /// </summary>
    /// <param name="branchCode"></param>
    /// <param name="currencyId"></param>
    /// <param name="clearingBranchCode"></param>
    /// <param name="clearingType"></param>
    /// <returns></returns>
    Task<bool> IsUniqueKeyExist(
        string branchCode,
        string currencyId,
        string clearingBranchCode,
        string clearingType
    );

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    IQueryable<AccountClearing> Table { get; }
}
