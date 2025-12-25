using LinqToDB;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.O24ACT.Common;
using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.O24ACT.Models;
using O24OpenAPI.O24ACT.Services.Interfaces;

namespace O24OpenAPI.O24ACT.Services;

/// <summary>
/// AccountCommonService
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="accountCommonRepository"></param>
/// <param name="localizationService"></param>
/// <param name="grpcAdminClient"></param>
/// <param name="staticCacheManager"></param>
public partial class AccountCommonService(
    IRepository<AccountCommon> accountCommonRepository,
    ILocalizationService localizationService,
    ICurrencyService currencyService,
    IStaticCacheManager staticCacheManager
) : IAccountCommonService
{
    #region  Fields
    private readonly IRepository<AccountCommon> _accountCommonRepository = accountCommonRepository;
    private readonly ILocalizationService _localizationService = localizationService;
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;
    private readonly ICurrencyService _currencyService = currencyService;

    #endregion
    #region Ctor

    #endregion
    /// <summary>
    /// GetById
    /// </summary>
    /// <param name="commonId"></param>
    /// <returns></returns>
    public virtual async Task<AccountCommon> GetById(int commonId)
    {
        return await _accountCommonRepository.GetById(commonId, cache => default);
    }

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<AccountCommon>> Search(SimpleSearchModel model)
    {
        IPagedList<AccountCommon> commons = await _accountCommonRepository.GetAllPaged(
            query =>
            {
                if (!string.IsNullOrEmpty(model.SearchText))
                {
                    query = query.Where(c =>
                        c.AccountNumber.Contains(model.SearchText, Constants.ICIC)
                        || c.AccountName.Contains(model.SearchText, Constants.ICIC)
                        || c.RefAccountNumber.Contains(model.SearchText, Constants.ICIC)
                        || c.RefAccountNumber2.Contains(model.SearchText, Constants.ICIC)
                    );
                }

                query = query.OrderBy(c => c.AccountName);
                return query;
            },
            model.PageIndex,
            model.PageSize
        );
        return commons;
    }

    /// <summary>
    /// model
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<AccountCommon>> Search(AccountCommonSearchModel model)
    {
        IPagedList<AccountCommon> commons = await _accountCommonRepository.GetAllPaged(
            query =>
            {
                if (!string.IsNullOrEmpty(model.AccountNumber))
                {
                    query = query.Where(c =>
                        c.AccountNumber.Contains(model.AccountNumber, Constants.ICIC)
                    );
                }

                if (!string.IsNullOrEmpty(model.AccountName))
                {
                    query = query.Where(c =>
                        c.AccountName.Contains(model.AccountName, Constants.ICIC)
                    );
                }

                if (!string.IsNullOrEmpty(model.RefAccountNumber))
                {
                    query = query.Where(c =>
                        c.RefAccountNumber.Contains(model.RefAccountNumber, Constants.ICIC)
                    );
                }

                if (!string.IsNullOrEmpty(model.RefAccountNumber2))
                {
                    query = query.Where(c =>
                        c.RefAccountNumber2.Contains(model.RefAccountNumber2, Constants.ICIC)
                    );
                }

                query = query.OrderBy(c => c.AccountName);
                return query;
            },
            model.PageIndex,
            model.PageSize
        );
        return commons;
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="common"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    public virtual async Task<AccountCommon> Create(AccountCommon common, string referenceId = "")
    {
        if (IsAccountNameExist(common.AccountName))
        {
            throw new O24OpenAPIException("Account Name is existing");
        }

        await common.Insert(referenceId);
        return common;
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="common"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    public virtual async Task Update(AccountCommon common, string referenceId = "")
    {
        if (common == null || common.Id == 0)
        {
            throw new O24OpenAPIException($"Common Account is not exist");
        }

        AccountCommon check = await GetByAccountName(common.AccountName);
        if (check != null && check.Id != common.Id)
        {
            throw new O24OpenAPIException("Account Name is existing");
        }

        await _accountCommonRepository.Update(common);
    }

    /// <summary>
    /// Check Account Name Exist
    /// </summary>
    /// <param name="acountName"></param>
    /// <returns></returns>
    public virtual bool IsAccountNameExist(string acountName)
    {
        AccountCommon common = _accountCommonRepository
            .Table.Where(c => c.AccountName.Equals(acountName))
            .FirstOrDefault();
        return common != null;
    }

    /// <summary>
    /// Get By Account Name
    /// </summary>
    /// <param name="accountName"></param>
    /// <returns></returns>
    public virtual async Task<AccountCommon> GetByAccountName(string accountName)
    {
        // var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(NeptuneEntityCacheDefaults<AccountCommon>.FunctionCacheKey, "GetByAccountName", accountName);
        // var account = await _staticCacheManager.Get(cacheKey, async () => {
        return await _accountCommonRepository
            .Table.Where(c => c.AccountName.Equals(accountName))
            .FirstOrDefaultAsync();
        // });
        // return account;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="accountName"></param>
    /// <param name="branchCode"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    public virtual async Task<string> GetAccountNumber(
        string accountName,
        string branchCode,
        string currencyCode
    )
    {
        AccountCommon accountCommon = await GetByAccountName(accountName);
        var accountNumber = accountCommon.AccountNumber;

        if (accountNumber.Contains("----"))
        {
            accountNumber = accountNumber.Replace("----", branchCode);
        }

        if (accountNumber.Contains("**") || accountNumber.Contains("?"))
        {
            GetCurrencyResponse currency = await _currencyService.GetCurrency(currencyCode);
            accountNumber = accountNumber.Replace("**", currency.ShortCurrencyId);
            accountNumber = accountNumber.Replace(
                "?",
                int.Parse(currency.ShortCurrencyId) < 9
                    ? int.Parse(currency.ShortCurrencyId).ToString()
                    : "9"
            );
        }
        return accountNumber;
    }
}
