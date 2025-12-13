using O24OpenAPI.Core;
using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.O24ACT.Models;
using O24OpenAPI.O24ACT.Models.Request;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.O24ACT.Services.Interfaces;

public partial interface IAccountChartService
{
    /// <summary>
    /// GetById
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<AccountChart> GetById(int id);

    /// <summary>
    ///
    /// </summary>
    /// <param name="level">zero if get all level</param>
    /// <returns></returns>
    Task<List<AccountChart>> GetAllCached(int level);

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<AccountChart>> Search(SimpleSearchModel model);

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<AccountChart>> Search(AccountChartSearchModel model);

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<AccountChart>> Lookup(AccountChartSearchModel model);

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="chart"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task<AccountChartCRUDReponseModel> Create(AccountChart chart, string referenceId = "");

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="chart"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task<AccountChartCRUDReponseModel> Update(AccountChart chart, string referenceId = "");

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="chart"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task Delete(AccountChart chart, string referenceId = "");

    /// <summary>
    /// Check Account Number Exist
    /// </summary>
    /// <param name="acno"></param>
    /// <returns></returns>
    bool IsAccountNumberExist(string acno);

    /// <summary>
    /// Get Account Chart By Account Number
    /// </summary>
    /// <param name="acno"></param>
    /// <returns></returns>
    Task<AccountChart> GetByAccountNumber(string acno);

    /// <summary>
    /// GetByAccountNumberPostingProcess
    /// </summary>
    /// <param name="acno"></param>
    /// <returns></returns>
    Task<AccountChartInforItemModel> GetByAccountNumberPostingProcess(string acno);

    /// <summary>
    /// OpenAccount
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <param name="accGrpIdx"></param>
    /// <param name="accIndexIdx"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task<AccountChart> OpenAccount(string accountNumber, int? accGrpIdx = null, int? accIndexIdx = null, string referenceId = "");
    /// <summary>
    /// OpenAccounPosting
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <param name="accGrpIdx"></param>
    /// <param name="accIndexIdx"></param>
    /// <param name="referenceId"></param>
    /// <param name="transId"></param>
    /// <returns></returns>
    Task<AccountChartInforItemModel> OpenAccountPosting(string accountNumber, int? accGrpIdx = null, int? accIndexIdx = null, string referenceId = "", string transId = "");
    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    Task<List<AccountChart>> GetAll();

    /// <summary>
    /// LookupByCurrency
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<AccountChart>> LookupByCurrency(LookupByCurrencyRuleFuncModel model);
    /// <summary>
    /// LookupByBranchCode
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<AccountChart>> LookupByBranchCode(LookupByBranchCodeRuleFuncModel model);
    /// <summary>
    /// LookupByBranchCodeDepositAccount
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<AccountChart>> LookupByBranchCodeDepositAccount(LookupByBranchCodeDepositAccountRuleFuncModel model);
    /// <summary>
    /// LookupByBranchCodeCurrencyCode
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<IPagedList<AccountChart>> LookupByBranchCodeCurrencyCode(LookupByBranchCodeCurrencyRuleFuncModel model);
    /// <summary>
    /// GetByBranchCodeCurrencyCode
    /// </summary>
    /// <param name="branchCode"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    List<AccountChartDepositGrpcModel> GetByBranchCodeCurrencyCode(string branchCode, string currencyCode);

    /// <summary>
    /// Get By BrandCode IsLeave And DirectPosting
    /// </summary>
    /// <param name="branchCode"></param>
    /// <param name="directPosting"></param>
    /// <param name="isLeave"></param>
    /// <returns></returns>
    Task<List<AccountChartDepositGrpcModel>> GetByBrandCodeIsLeaveAndDirectPosting(string branchCode, string directPosting, bool isLeave);

    /// <summary>
    /// List clearing account for moving profit - loss to retail earnings
    /// </summary>
    /// <returns></returns>
    Task<List<MoveProfitToRetailModel>> ListMoveProfitToRetailAccounts();

    /// <summary>
    /// List income/expense close accounts
    /// </summary>
    /// <returns></returns>
    Task<List<MoveProfitToRetailModel>> LisIncomeExpenseCloseAccounts();

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Task CalculateBalance();

    /// <summary>
    ///
    /// </summary>
    /// <value></value>
    IQueryable<AccountChart> Table { get; }
    /// <summary>
    /// CreateAsync
    /// </summary>
    /// <param name="model"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    Task<AccountChartCRUDReponseModel> CreateAsync(CreateAccountChartRequestModel model);
    /// <summary>
    /// Delete Async
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(AccountChartDefaultModel model);
}
