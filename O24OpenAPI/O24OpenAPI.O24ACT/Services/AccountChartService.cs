using System.Text.Json;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.O24ACT.Common;
using O24OpenAPI.O24ACT.Configuration;
using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.O24ACT.Models;
using O24OpenAPI.O24ACT.Models.Request;
using O24OpenAPI.O24ACT.Services.Interfaces;
using O24OpenAPI.Web.Framework.Exceptions;
using O24OpenAPI.Web.Framework.Infrastructure.Mapper.Extensions;
using O24OpenAPI.Web.Framework.Localization;
using O24OpenAPI.Web.Framework.Models;
using O24OpenAPI.Core.SeedWork;

namespace O24OpenAPI.O24ACT.Services;

/// <summary>
/// AccountChartService
/// </summary>
/// <remarks>
/// Constructor
/// </remarks>
/// <param name="accountChartRepository"></param>
/// <param name="accountBalanceService"></param>
/// <param name="accountCommonService"></param>
/// <param name="accountingSeting"></param>
/// <param name="localizationService"></param>
/// <param name="checkingAccountRulesService"></param>
/// <param name="grpcAdminClient"></param>
/// <param name="grpcDepositClient"></param>
/// <param name="staticCacheManager"></param>
public partial class AccountChartService(
    IRepository<AccountChart> accountChartRepository,
    IAccountBalanceService accountBalanceService,
    IAccountCommonService accountCommonService,
    AccountingSettings accountingSeting,
    ILocalizationService localizationService,
    ICheckingAccountRulesService checkingAccountRulesService,
    IStaticCacheManager staticCacheManager,
    IBranchService branchService,
    ICurrencyService currencyService
) : IAccountChartService
{
    #region  Fields
    private readonly IRepository<AccountChart> _accountChartRepository = accountChartRepository;
    private readonly IAccountBalanceService _accountBalanceService = accountBalanceService;
    private readonly IAccountCommonService _accountCommonService = accountCommonService;
    private readonly ICheckingAccountRulesService _checkingAccountRulesService =
        checkingAccountRulesService;
    private readonly AccountingSettings _accountingSeting = accountingSeting;
    private readonly ILocalizationService _localizationService = localizationService;
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;
    private readonly ICurrencyService _currencyService = currencyService;
    private readonly IBranchService _branchService = branchService;

    #endregion
    #region Ctor

    #endregion
    /// <summary>
    /// Get By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<AccountChart> GetById(int id)
    {
        return await _accountChartRepository.GetById(id, cache => default);
    }

    /// <summary>
    /// GetAll
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<AccountChart>> GetAll()
    {
        return (
            await _accountChartRepository.GetAll(query =>
            {
                return query;
            })
        ).ToList();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="level">zero if get all level</param>
    /// <returns></returns>
    public virtual async Task<List<AccountChart>> GetAllCached(int level)
    {
        // var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(NeptuneEntityCacheDefaults<AccountChart>.FunctionCacheKey, "GetAllCached", $"{level}");
        // var accounts = await _staticCacheManager.Get(cacheKey, async () =>
        // {
        var query = _accountChartRepository.Table;
        if (level > 0)
        {
            query = query.Where(a => a.AccountLevel == level);
        }

        return await query.OrderBy(a => a.AccountNumber).ToListAsync();
        // });
        // return accounts;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="levelFrom"></param>
    /// <param name="levelTo"></param>
    /// <param name="clearCache"></param>
    /// <returns></returns>
    public virtual async Task<List<AccountChart>> GetAllCached(
        int levelFrom,
        int? levelTo,
        bool clearCache
    )
    {
        var cacheKey = new CacheKey("GetAllCached", $"{levelFrom}.{levelTo}");
        if (clearCache)
        {
            await _staticCacheManager.Remove(cacheKey);
        }

        var accounts = await _staticCacheManager.Get(
            cacheKey,
            async () =>
            {
                var query = _accountChartRepository.Table;
                query = query.Where(c => c.AccountLevel >= levelFrom && c.AccountLevel <= levelTo);
                if (levelTo.HasValue)
                {
                    query = query.Where(c => c.AccountLevel <= levelTo);
                }

                return await query.OrderBy(a => a.AccountNumber).ToListAsync();
            }
        );
        return accounts;
    }

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<AccountChart>> Search(SimpleSearchModel model)
    {
        var charts = await _accountChartRepository.GetAllPaged(
            query =>
            {
                if (!string.IsNullOrEmpty(model.SearchText))
                {
                    query = query.Where(c =>
                        c.AccountNumber.Contains(model.SearchText, Constants.ICIC)
                        || c.AccountLevel.ToString().Equals(model.SearchText)
                        || c.CurrencyCode.Equals(model.SearchText)
                        || c.AccountName.Contains(model.SearchText, Constants.ICIC)
                        || c.AccountClassification.Equals(model.SearchText)
                        || c.BalanceSide.Equals(model.SearchText)
                        || c.AccountGroup.Contains(model.SearchText, Constants.ICIC)
                    );
                }
                query = query
                    .OrderBy(c => c.AccountLevel)
                    .ThenBy(c => c.AccountNumber)
                    .ThenBy(c => c.CurrencyCode);
                return query;
            },
            model.PageIndex,
            model.PageSize
        );

        return charts;
    }

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<AccountChart>> LookupByCurrency(
        LookupByCurrencyRuleFuncModel model
    )
    {
        var charts = await _accountChartRepository.GetAllPaged(
            query =>
            {
                if (!string.IsNullOrEmpty(model.CurrencyCode))
                {
                    query = query.Where(c =>
                        c.CurrencyCode.Equals(model.CurrencyCode)
                        && c.DirectPosting.Equals(Constants.DirectPosting.Yes)
                        && c.IsAccountLeave
                        && c.AccountLevel == 9
                    );
                }

                if (!string.IsNullOrEmpty(model.NotInAccountGroup))
                {
                    query = query.Where(c =>
                        !c.AccountGroup.Contains(model.NotInAccountGroup, Constants.ICIC)
                    );
                }

                query = query
                    .OrderBy(c => c.AccountLevel)
                    .ThenBy(c => c.AccountNumber)
                    .ThenBy(c => c.CurrencyCode);
                return query;
            },
            model.PageIndex,
            model.PageSize
        );
        return charts;
    }

    /// <summary>
    /// LookupByBranchCode
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<AccountChart>> LookupByBranchCode(
        LookupByBranchCodeRuleFuncModel model
    )
    {
        var charts = await _accountChartRepository.GetAllPaged(
            query =>
            {
                query = query.Where(c =>
                    c.BranchCode.Equals(model.AccountBranchCode)
                    && c.DirectPosting.Equals(Constants.DirectPosting.Yes)
                    && c.IsAccountLeave
                    && c.AccountLevel == 9
                );

                if (!string.IsNullOrEmpty(model.NotInAccountGroup))
                {
                    query = query.Where(c =>
                        !c.AccountGroup.Contains(model.NotInAccountGroup, Constants.ICIC)
                    );
                }

                query = query
                    .OrderBy(c => c.AccountLevel)
                    .ThenBy(c => c.AccountNumber)
                    .ThenBy(c => c.CurrencyCode);
                return query;
            },
            model.PageIndex,
            model.PageSize
        );
        return charts;
    }

    /// <summary>
    /// LookupByBranchCode
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<AccountChart>> LookupByBranchCodeCurrencyCode(
        LookupByBranchCodeCurrencyRuleFuncModel model
    )
    {
        var charts = await _accountChartRepository.GetAllPaged(
            query =>
            {
                query = query.Where(c =>
                    c.BranchCode.Equals(model.AccountBranchCode)
                    && c.CurrencyCode.Equals(model.CurrencyCode)
                    && c.DirectPosting.Equals(Constants.DirectPosting.Yes)
                    && c.IsAccountLeave
                    && c.AccountLevel == 9
                );

                if (!string.IsNullOrEmpty(model.NotInAccountGroup))
                {
                    query = query.Where(c =>
                        !c.AccountGroup.Contains(model.NotInAccountGroup, Constants.ICIC)
                    );
                }

                query = query
                    .OrderBy(c => c.AccountLevel)
                    .ThenBy(c => c.AccountNumber)
                    .ThenBy(c => c.CurrencyCode);
                return query;
            },
            model.PageIndex,
            model.PageSize
        );
        return charts;
    }

    /// <summary>
    /// GetByBranchCodeCurrencyCode
    /// </summary>
    /// <param name="branchCode"></param>
    /// <param name="currencyCode"></param>
    /// <returns></returns>
    /// <exception cref="NeptuneException"></exception>
    public virtual List<AccountChartDepositGrpcModel> GetByBranchCodeCurrencyCode(
        string branchCode,
        string currencyCode
    )
    {
        var charts = (
            from c in _accountChartRepository.Table
            where
                c.BranchCode == branchCode
                && c.CurrencyCode == currencyCode
                && c.IsAccountLeave
                && c.DirectPosting == Constants.DirectPosting.Yes
                && c.AccountLevel == 9
            select new AccountChartDepositGrpcModel
            {
                AccountNumber = c.AccountNumber,
                AccountName = c.AccountName,
                CurrencyCode = c.CurrencyCode,
                IsAccountLeave = c.IsAccountLeave,
                DirectPosting = c.DirectPosting,
                DepositStatus = string.Empty,
                DepositType = string.Empty,
                BranchCode = c.BranchCode,
                RecBy = Constants.Module.Accounting,
            }
        ).ToList();
        return charts;
    }

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<AccountChart>> Search(AccountChartSearchModel model)
    {
        var paramKey = Newtonsoft.Json.JsonConvert.SerializeObject(model);

        var cacheKey = new CacheKey($"Search {paramKey}");

        var accounts = await _staticCacheManager.Get(
            cacheKey,
            async () =>
            {
                var charts = await _accountChartRepository.GetAllPaged(
                    query =>
                    {
                        var filterLevel = 0;
                        query = query.Search(
                            nameof(AccountChart.AccountNumber),
                            model.AccountNumber
                        );

                        if (
                            model.AccountLevelFrom.HasValue
                            && model.AccountLevelTo.HasValue
                            && model.AccountLevelFrom.HasValue == model.AccountLevelTo.HasValue
                        )
                        {
                            filterLevel = model.AccountLevelFrom ?? 0;
                            query = query.Where(c => c.AccountLevel == filterLevel);
                        }
                        else
                        {
                            if (model.AccountLevelFrom.HasValue)
                            {
                                query = query.Where(c => c.AccountLevel >= model.AccountLevelFrom);
                            }

                            if (model.AccountLevelTo.HasValue)
                            {
                                query = query.Where(c => c.AccountLevel <= model.AccountLevelTo);
                            }
                        }

                        if (!string.IsNullOrEmpty(model.CurrencyCode))
                        {
                            query = query.Where(c => c.CurrencyCode.Equals(model.CurrencyCode));
                        }

                        query = query.Search(nameof(AccountChart.AccountName), model.AccountName);
                        query = query.Search(
                            nameof(AccountChart.AccountClassification),
                            model.AccountClassification
                        );
                        query = query.Search(nameof(AccountChart.BalanceSide), model.BalanceSide);
                        query = query.Search(nameof(AccountChart.AccountGroup), model.AccountGroup);
                        if (!string.IsNullOrEmpty(model.NotInAccountGroup))
                        {
                            query = query.Where(c =>
                                !c.AccountGroup.Contains(model.NotInAccountGroup, Constants.ICIC)
                            );
                        }

                        if (filterLevel == 0)
                        {
                            query = query
                                .OrderBy(c => c.AccountLevel)
                                .ThenBy(c => c.AccountNumber)
                                .ThenBy(c => c.CurrencyCode);
                        }
                        else
                        {
                            query = query.OrderBy(c => c.AccountNumber);
                        }

                        return query;
                    },
                    model.PageIndex,
                    model.PageSize
                );
                return charts;
            }
        );
        return accounts;
    }

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<AccountChart>> Lookup(AccountChartSearchModel model)
    {
        var paramKey = $"";
        if (model.AccountNumber.HasValue())
        {
            paramKey += $".{model.AccountNumber}";
        }

        if (model.AccountName.HasValue())
        {
            paramKey += $".{model.AccountName}";
        }

        var cacheKey = new CacheKey($"Lookup {paramKey}");

        if (model.RefreshCache.HasValue && model.RefreshCache.Value)
        {
            await _staticCacheManager.Remove(cacheKey);
        }

        // var accounts = await _staticCacheManager.Get(cacheKey, async () =>
        // {
        // Console.WriteLine("get from db");
        var charts = await _accountChartRepository.GetAllPaged(
            query =>
            {
                var filterLevel = 0;
                query = query.Search(nameof(AccountChart.AccountNumber), model.AccountNumber);

                if (
                    model.AccountLevelFrom.HasValue
                    && model.AccountLevelTo.HasValue
                    && model.AccountLevelFrom.HasValue == model.AccountLevelTo.HasValue
                )
                {
                    filterLevel = model.AccountLevelFrom ?? 0;
                    query = query.Where(c => c.AccountLevel == filterLevel);
                }
                else
                {
                    if (model.AccountLevelFrom.HasValue)
                    {
                        query = query.Where(c => c.AccountLevel >= model.AccountLevelFrom);
                    }

                    if (model.AccountLevelTo.HasValue)
                    {
                        query = query.Where(c => c.AccountLevel <= model.AccountLevelTo);
                    }
                }

                if (!string.IsNullOrEmpty(model.CurrencyCode))
                {
                    query = query.Where(c => c.CurrencyCode.Equals(model.CurrencyCode));
                }

                query = query.Search(nameof(AccountChart.AccountName), model.AccountName);
                query = query.Search(
                    nameof(AccountChart.AccountClassification),
                    model.AccountClassification
                );
                query = query.Search(nameof(AccountChart.BalanceSide), model.BalanceSide);
                query = query.Search(nameof(AccountChart.AccountGroup), model.AccountGroup);
                // slow
                if (!string.IsNullOrEmpty(model.NotInAccountGroup))
                {
                    query = query.Where(c => model.NotInAccountGroup != c.AccountGroup);
                }

                if (!string.IsNullOrEmpty(model.NotInDirectPosting))
                {
                    query = query.Where(c => model.NotInDirectPosting != c.DirectPosting);
                }

                if (!string.IsNullOrEmpty(model.DirectPosting))
                {
                    query = query.Where(c => model.DirectPosting == c.DirectPosting);
                }

                if (model.IsAccountLeave.HasValue)
                {
                    query = query.Where(c => model.IsAccountLeave.Value == c.IsAccountLeave);
                }

                if (filterLevel == 0)
                {
                    query = query
                        .OrderBy(c => c.AccountLevel)
                        .ThenBy(c => c.AccountNumber)
                        .ThenBy(c => c.CurrencyCode);
                }
                else
                {
                    query = query.OrderBy(c => c.AccountNumber);
                }

                return query;
            },
            model.PageIndex,
            model.PageSize
        );

        return charts;
        // });
        // return accounts;
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="chart"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    /// <exception cref="NeptuneException"></exception>
    /// <exception cref="Exception"></exception>
    public virtual async Task<AccountChartCRUDReponseModel> Create(
        AccountChart chart,
        string referenceId = ""
    )
    {
        if (IsAccountNumberExist(chart.AccountNumber))
        {
            throw new O24OpenAPIException("Bank Account Number " + chart.AccountNumber);
        }

        if (chart.AccountLevel == 8 || chart.AccountLevel == 9)
        {
            var currency = await _currencyService.GetCurrency(chart.CurrencyCode);
            if (currency.Id == 0)
            {
                throw new O24OpenAPIException("Currency", chart.CurrencyCode);
            }
        }

        await _checkingAccountRulesService.CheckingRuleAccount(
            chart.AccountClassification,
            chart.ReverseBalance,
            chart.BalanceSide,
            chart.PostingSide,
            chart.AccountGroup,
            chart.AccountCategories
        );
        int sysLeaveLevel = _accountingSeting.AccountLeaveLevel;

        chart.ParentAccountId = chart.AccountLevel switch
        {
            9 => chart.AccountNumber.Substring(
                _accountingSeting.LengthBranch,
                chart.AccountNumber.Length - _accountingSeting.LengthBranch
            ),
            1 => null,
            _ => chart.AccountNumber.Substring(0, chart.AccountNumber.Length - 2),
        };
        await chart.Insert(referenceId);
        if (chart.AccountLevel.Equals(sysLeaveLevel))
        {
            AccountBalance accountBalance = new AccountBalance
            {
                AccountNumber = chart.AccountNumber,
                BranchCode = chart.BranchCode,
                CurrencyCode = chart.CurrencyCode,
            };
            await accountBalance.Insert();
            chart.IsAccountLeave = true;
            await _accountChartRepository.Update(chart, referenceId);
        }
        var chartResult = chart.ToModel<AccountChartCRUDReponseModel>();
        chartResult.MultiValueNameLang = JsonSerializer.Deserialize<MultiValueName>(
            chart.MultiValueName
        );
        return chartResult;
    }

    /// <summary>
    /// OpenAccount
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <param name="accGrpIdx"></param>
    /// <param name="accIndexIdx"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    public virtual async Task<AccountChart> OpenAccount(
        string accountNumber,
        int? accGrpIdx = null,
        int? accIndexIdx = null,
        string referenceId = ""
    )
    {
        int lenghBranch = _accountingSeting.LengthBranch;
        if (
            accountNumber.Length != _accountingSeting.LengthAccountNumber
            || !double.TryParse(accountNumber, out _)
        )
        {
            throw new O24OpenAPIException(
                String.Format(
                    "Auto open account [{0} - {1} - {2}] must be number and lenght be [{3}]",
                    accountNumber,
                    accGrpIdx,
                    accIndexIdx,
                    _accountingSeting.LengthAccountNumber
                )
            );
        }

        var accountParent = await GetByAccountNumber(accountNumber.Substring(lenghBranch + 2, 7));
        if (accountParent == null)
        {
            throw new O24OpenAPIException($"Bank Account Definition {accountNumber} is not exist");
        }

        var accountChild = accountParent;
        var currency = await _currencyService.GetCurrencyByShortId(
            accountNumber.Substring(lenghBranch, 2)
        );
        if (currency.Id == 0 || currency.StatusOfCurrency != "A")
        {
            throw new O24OpenAPIException($"Currency {accountNumber.Substring(lenghBranch, 2)}");
        }

        var branch = await _branchService.GetBranchByBranchCode(accountNumber[..lenghBranch]);
        if (branch.Id == 0)
        {
            throw new O24OpenAPIException($"Branch {accountNumber[..lenghBranch]} is not exist");
        }

        accountChild.AccountNumber = accountNumber;
        accountChild.AccountName = accountParent.AccountName;
        accountChild.ShortAccountName = accountParent.ShortAccountName;
        accountChild.MultiValueName = accountParent.MultiValueName;
        accountChild.ParentAccountId = accountNumber.Substring(lenghBranch + 2, 7);
        accountChild.AccountLevel = _accountingSeting.AccountLeaveLevel;
        accountChild.CurrencyCode = currency.CurrencyId;
        accountChild.BranchCode = branch.BranchCode;
        accountChild.IsAccountLeave = true;
        accountChild.ReferencesNumber = "AUTOOPEN";
        accountChild.DirectPosting = "Y";
        accountChild.IsMultiCurrency = "N";
        accountChild.AccountCategories = "N";

        //accountChild.AccountNumber = $"{accountChild.BranchCode.ToString().PadLeft(5, '0')}{accountChild.CurrencyCode.ToString()}{accountChild.AccountNumber.Substring(6)}";
        await accountChild.Insert(referenceId);
        AccountBalance accountBalance = new AccountBalance
        {
            AccountNumber = accountChild.AccountNumber,
            BranchCode = accountChild.BranchCode,
            CurrencyCode = accountChild.CurrencyCode,
        };
        await accountBalance.Insert();
        return accountChild;
    }

    /// <summary>
    /// OpenAccountPosting
    /// </summary>
    /// <param name="accountNumber"></param>
    /// <param name="accGrpIdx"></param>
    /// <param name="accIndexIdx"></param>
    /// <param name="referenceId"></param>
    /// <param name="transId"></param>
    /// <returns></returns>
    public virtual async Task<AccountChartInforItemModel> OpenAccountPosting(
        string accountNumber,
        int? accGrpIdx = null,
        int? accIndexIdx = null,
        string referenceId = "",
        string transId = ""
    )
    {
        int lenghBranch = _accountingSeting.LengthBranch;
        var result = new AccountChartInforItemModel();
        if (
            accountNumber.Length != _accountingSeting.LengthAccountNumber
            || !double.TryParse(accountNumber, out _)
        )
        {
            throw new O24OpenAPIException(
                string.Format(
                    "Auto open account [{0} - {1} - {2}] must be number and lenght be [{3}]",
                    accountNumber,
                    accGrpIdx,
                    accIndexIdx,
                    _accountingSeting.LengthAccountNumber
                )
            );
        }

        var accountParent =
            await GetByAccountNumber(accountNumber.Substring(lenghBranch + 2, 7))
            ?? throw new O24OpenAPIException(
                $"Bank Account Definition {accountNumber} is not exist"
            );
        var accountChild = accountParent;
        var currency = await _currencyService.GetCurrencyByShortId(
            accountNumber.Substring(lenghBranch, 2)
        );
        if (currency.Id == 0 || currency.StatusOfCurrency != "A")
        {
            throw new O24OpenAPIException($"Currency {accountNumber.Substring(lenghBranch, 2)}");
        }

        var branch = await _branchService.GetBranchByBranchCode(
            accountNumber.Substring(0, lenghBranch)
        );
        if (branch.Id == 0)
        {
            throw new O24OpenAPIException($"Currency {accountNumber.Substring(0, lenghBranch)}");
        }

        accountChild.AccountNumber = accountNumber;
        accountChild.AccountName = accountParent.AccountName;
        accountChild.ShortAccountName = accountParent.ShortAccountName;
        accountChild.MultiValueName = accountParent.MultiValueName;
        accountChild.ParentAccountId = accountNumber.Substring(lenghBranch + 2, 7);
        accountChild.AccountLevel = _accountingSeting.AccountLeaveLevel;
        accountChild.CurrencyCode = currency.CurrencyId;
        accountChild.BranchCode = branch.BranchCode;
        accountChild.IsAccountLeave = true;
        accountChild.ReferencesNumber = "AUTOOPEN for transaction " + transId;
        accountChild.AccountCategories = "N";
        accountChild.DirectPosting = "Y";
        accountChild.IsMultiCurrency = "N";
        // accountChild.AccountNumber = $"{accountChild.BranchCode.ToString().PadLeft(5, '0')}{accountChild.CurrencyCode.ToString()}{accountChild.AccountNumber.Substring(lenghBranch + 2)}";
        await accountChild.Insert(referenceId);
        AccountBalance accountBalance = new AccountBalance
        {
            AccountNumber = accountChild.AccountNumber,
            BranchCode = accountChild.BranchCode,
            CurrencyCode = accountChild.CurrencyCode,
        };
        await accountBalance.Insert();
        result.AccountChart = accountChild;
        result.AccountBalance = accountBalance;
        return result;
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="chart"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NeptuneException"></exception>
    public virtual async Task<AccountChartCRUDReponseModel> Update(
        AccountChart chart,
        string referenceId = ""
    )
    {
        if (chart == null || chart.Id == 0)
        {
            throw new ArgumentNullException("Bank Account Definition is not exist");
        }

        await _checkingAccountRulesService.CheckingRuleAccount(
            chart.AccountClassification,
            chart.ReverseBalance,
            chart.BalanceSide,
            chart.PostingSide,
            chart.AccountGroup,
            chart.AccountCategories
        );
        if (chart.IsVisible.ToUpper().Equals(Constants.Invisible.Yes))
        {
            var isBalanceZero = await _accountBalanceService.IsBalanceEqualZero(
                chart.AccountNumber
            );
            if (!isBalanceZero)
            {
                throw new O24OpenAPIException(
                    string.Format(
                        await _localizationService.GetResource(
                            "Accounting.Validation.ACT_INVALID_INACTIVE_ACC"
                        ),
                        chart.Id
                    )
                );
            }
        }
        var check = await GetByAccountNumber(chart.AccountNumber);
        if (check != null && check.Id != chart.Id)
        {
            throw new O24OpenAPIException("Bank Account Number is not exist");
        }

        await _accountChartRepository.Update(chart, referenceId);
        var chartResult = chart.ToModel<AccountChartCRUDReponseModel>();
        chartResult.MultiValueNameLang = JsonSerializer.Deserialize<MultiValueName>(
            chart.MultiValueName
        );
        return chartResult;
    }

    /// <summary>
    /// delete
    /// </summary>
    /// <param name="chart"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    public virtual async Task Delete(AccountChart chart, string referenceId = "")
    {
        if (chart == null || chart.Id == 0)
        {
            throw new ArgumentNullException("Bank Account Definition is not exist");
        }

        if (chart.IsAccountLeave)
        {
            AccountBalance accBalance = await _accountBalanceService.GetByAccountNumber(
                chart.AccountNumber
            );
            if (
                !(
                    accBalance.DailyCredit == 0
                    && accBalance.WeeklyCredit == 0
                    && accBalance.WeeklyDebit == 0
                    && accBalance.MonthlyCredit == 0
                    && accBalance.MonthlyDebit == 0
                    && accBalance.QuarterlyCredit == 0
                    && accBalance.QuarterlyDebit == 0
                    && accBalance.HalfYearlyCredit == 0
                    && accBalance.HalfYearlyDebit == 0
                    && accBalance.YearlyCredit == 0
                    && accBalance.YearlyDebit == 0
                    && accBalance.WeekAverageBalance == 0
                    && accBalance.MonthAverageBalance == 0
                    && accBalance.HalfYearAverageBalance == 0
                    && accBalance.HalfYearAverageBalance == 0
                )
            )
            {
                throw new O24OpenAPIException(
                    string.Format(
                        await _localizationService.GetResource(
                            "Accounting.Validation.ACT_INVALID_INACTIVE_ACC"
                        ),
                        chart.AccountNumber
                    )
                );
            }

            await accBalance.Delete();
        }
        await chart.Delete(referenceId);
    }

    /// <summary>
    /// Check Account Number Exist
    /// </summary>
    /// <param name="acno"></param>
    /// <returns></returns>
    public virtual bool IsAccountNumberExist(string acno)
    {
        var accountchart = _accountChartRepository
            .Table.Where(c => c.AccountNumber.Equals(acno))
            .FirstOrDefault();
        return (accountchart != null);
    }

    /// <summary>
    /// Get Account Chart By Account Number
    /// </summary>
    /// <param name="acno"></param>
    /// <returns></returns>
    public virtual async Task<AccountChart> GetByAccountNumber(string acno)
    {
        var cacheKey = new CacheKey(acno);
        var account = await _staticCacheManager.Get(
            cacheKey,
            async () =>
            {
                return await _accountChartRepository
                    .Table.Where(c => c.AccountNumber == acno)
                    .FirstOrDefaultAsync();
            }
        );
        return account;
    }

    /// <summary>
    /// GetByAccountNumberPostingProcess
    /// </summary>
    /// <param name="acno"></param>
    /// <returns></returns>
    public virtual async Task<AccountChartInforItemModel> GetByAccountNumberPostingProcess(
        string acno
    )
    {
        var result = new AccountChartInforItemModel();
        var accountChart =
            await GetByAccountNumber(acno)
            ?? throw new O24OpenAPIException($"Account number {acno} is not exit");
        result.AccountChart = accountChart;
        result.AccountBalance = await _accountBalanceService.GetByAccountNumber(
            accountChart.AccountNumber
        );
        return result;
    }

    /// <summary>
    /// Get By BrandCode IsLeave And DirectPosting
    /// </summary>
    /// <param name="branchCode"></param>
    /// <param name="directPosting"></param>
    /// <param name="isLeave"></param>
    /// <returns></returns>
    public virtual async Task<
        List<AccountChartDepositGrpcModel>
    > GetByBrandCodeIsLeaveAndDirectPosting(string branchCode, string directPosting, bool isLeave)
    {
        var result = await (
            from d in _accountChartRepository.Table.DefaultIfEmpty()
            where
                (
                    d.BranchCode == branchCode
                    && d.IsAccountLeave == isLeave
                    && d.AccountLevel == 9
                    && d.DirectPosting == directPosting
                )
            select new AccountChartDepositGrpcModel
            {
                AccountNumber = d.AccountNumber,
                AccountName = d.AccountName,
                CurrencyCode = d.CurrencyCode,
                IsAccountLeave = d.IsAccountLeave,
                DirectPosting = d.DirectPosting,
                DepositStatus = string.Empty,
                DepositType = string.Empty,
                BranchCode = d.BranchCode,
                RecBy = Constants.Module.Accounting,
            }
        ).ToListAsync();
        return result;
    }

    /// <summary>
    /// List clearing account for moving profit - loss to retail earnings
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<MoveProfitToRetailModel>> ListMoveProfitToRetailAccounts()
    {
        string accountName = "RETAIL_EARNINGS";

        var query =
            from a in _accountChartRepository.Table.Where(a => a.AccountGroup == "L")
            join b in _accountBalanceService.Table.Where(b => b.Balance != 0)
                on a.AccountNumber equals b.AccountNumber
            select new MoveProfitToRetailModel
            {
                AccountType = a.AccountGroup,
                DebitAccount = a.AccountNumber,
                Balance = b.Balance,
                ProfitAccount = "",
                BranchCode = a.BranchCode,
                CurrencyCode = a.CurrencyCode,
            };
        var accounts = await query.ToListAsync();
        foreach (var account in accounts)
        {
            account.ProfitAccount = await _accountCommonService.GetAccountNumber(
                accountName,
                account.BranchCode,
                account.CurrencyCode
            );
        }

        return accounts;
    }

    /// <summary>
    /// List income/expense close accounts
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<MoveProfitToRetailModel>> LisIncomeExpenseCloseAccounts()
    {
        string accountName = "PROFIT";

        var query =
            from a in _accountChartRepository.Table.Where(a => "I,E".Contains(a.AccountGroup))
            join b in _accountBalanceService.Table.Where(b => b.Balance != 0)
                on a.AccountNumber equals b.AccountNumber
            select new MoveProfitToRetailModel
            {
                AccountType = a.AccountGroup,
                DebitAccount = a.AccountNumber,
                Balance = b.Balance,
                ProfitAccount = "",
                BranchCode = a.BranchCode,
                CurrencyCode = a.CurrencyCode,
            };
        var accounts = await query.ToListAsync();
        foreach (var account in accounts)
        {
            account.ProfitAccount = await _accountCommonService.GetAccountNumber(
                accountName,
                account.BranchCode,
                account.CurrencyCode
            );
        }

        return accounts;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public virtual async Task CalculateBalance()
    {
        var queryChart = _accountChartRepository.Table.Where(c => c.AccountLevel == 9);
        var charts = await (from c in queryChart select c).ToListAsync();
    }

    public Task<IPagedList<AccountChart>> LookupByBranchCodeDepositAccount(
        LookupByBranchCodeDepositAccountRuleFuncModel model
    )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Create Account Chart
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="O24OpenAPIException"></exception>
    public async Task<AccountChartCRUDReponseModel> CreateAsync(
        CreateAccountChartRequestModel model
    )
    {
        ArgumentNullException.ThrowIfNull(model);

        if (IsAccountNumberExist(model.AccountNumber))
        {
            throw await O24Exception.CreateAsync(
                O24ActResourceCode.Account.AccountNumberIsExisting,
                model.Language,
                [model.AccountNumber]
            );
        }

        var level = model.AccountLevel ?? 0;

        if (level == 8 || level == 9)
        {
            var currency = await _currencyService.GetCurrency(model.CurrencyCode);
            if (currency.Id == 0)
            {
                throw new NotFoundException("Currency", model.CurrencyCode);
            }
        }

        await _checkingAccountRulesService.CheckingRuleAccount(
            model.AccountClassification,
            model.ReverseBalance,
            model.BalanceSide,
            model.PostingSide,
            model.AccountGroup,
            model.AccountCategories
        );

        int sysLeaveLevel = _accountingSeting.AccountLeaveLevel;

        var multiValueNameJson = JsonSerializer.Serialize(new { model.LaosName });

        var chart = new AccountChart
        {
            AccountCategories = model.AccountCategories,
            AccountClassification = model.AccountClassification,
            AccountGroup = model.AccountGroup,
            AccountName = model.AccountName,
            AccountNumber = model.AccountNumber,
            BalanceSide = model.BalanceSide,
            BranchCode = model.BranchCode,
            CurrencyCode = model.CurrencyCode,
            DirectPosting = model.DirectPosting,
            IsVisible = model.IsVisible,
            JobProcessOption = model.JobProcessOption,
            PostingSide = model.PostingSide,
            ReverseBalance = model.ReverseBalance,
            ShortAccountName = model.ShortAccountName,
            AccountLevel = level,
            MultiValueName = multiValueNameJson,
            CreatedOnUtc = DateTime.UtcNow,
            UpdatedOnUtc = DateTime.UtcNow,
            IsMultiCurrency = Constants.IsMultiCurrency.Yes,
        };

        chart.ParentAccountId = level switch
        {
            9 => SafeSlice(chart.AccountNumber, _accountingSeting.LengthBranch, null),
            1 => null,
            _ => SafeSlice(chart.AccountNumber, 0, Math.Max(chart.AccountNumber.Length - 2, 0)),
        };

        if (chart.AccountLevel.Equals(sysLeaveLevel))
        {
            var accountBalance = new AccountBalance
            {
                AccountNumber = chart.AccountNumber,
                BranchCode = chart.BranchCode,
                CurrencyCode = chart.CurrencyCode,
            };
            await accountBalance.Insert();

            chart.IsAccountLeave = true;
        }

        await _accountChartRepository.InsertAsync(chart);

        var chartResult = chart.ToAccountChartCRUDReponseModel();
        chartResult.MultiValueNameLang = JsonSerializer.Deserialize<MultiValueName>(
            chart.MultiValueName
        );

        return chartResult;

        static string SafeSlice(string input, int? start, int? end)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            int s = Math.Clamp(start ?? 0, 0, input.Length);
            int e = Math.Clamp(end ?? input.Length, 0, input.Length);
            if (e < s)
            {
                e = s;
            }

            return input[s..e];
        }
    }

    public async Task<bool> DeleteAsync(AccountChartDefaultModel model)
    {
        var chart = await GetByAccountNumber(model.AccountNumber);
        await Delete(chart, model.RefId);
        return true;
    }

    /// <summary>
    ///
    /// </summary>
    public virtual IQueryable<AccountChart> Table => _accountChartRepository.Table;
}
