using O24OpenAPI.Core;
using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Data.Extensions;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Localization;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.O24ACT.Common;
using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.O24ACT.Models;
using O24OpenAPI.O24ACT.Services.Interfaces;

namespace O24OpenAPI.O24ACT.Services;

/// <summary>
/// Constructor
/// </summary>
/// <param name="accountClearingRepository"></param>
/// <param name="localizationService"></param>
/// <param name="grpcAdminClient"></param>
/// <param name="accountChartService"></param>
public partial class AccountClearingService(
    IRepository<AccountClearing> accountClearingRepository,
    ILocalizationService localizationService,
    IAccountChartService accountChartService,
    IBranchService branchService
) : IAccountClearingService
{
    #region  Fields
    private readonly ILocalizationService _localizationService = localizationService;
    private readonly IAccountChartService _accountChartService = accountChartService;
    private readonly IRepository<AccountClearing> _accountClearingRepository =
        accountClearingRepository;
    private readonly IBranchService _branchService = branchService;

    #endregion
    #region Ctor

    #endregion
    /// <summary>
    /// GetById
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<AccountClearing> GetById(int id)
    {
        return await _accountClearingRepository.GetById(id, cache => default);
    }

    /// <summary>
    /// View
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<AccountClearingViewReponseModel> View(int id)
    {
        var clearing = await GetById(id);
        if (clearing == null)
        {
            return null;
        }

        var branchOrginal = await _branchService.GetBranchByBranchCode(clearing.BranchCode);
        var clearingBranch = await _branchService.GetBranchByBranchCode(
            clearing.ClearingBranchCode
        );
        var accountChart = await _accountChartService.GetByAccountNumber(clearing.AccountNumber);
        var _clearingReponse = clearing.ToAccountClearingViewReponseModel();
        _clearingReponse.BranchName = branchOrginal?.BranchName ?? null;
        _clearingReponse.ClearingBranchName = clearingBranch?.BranchName ?? null;
        _clearingReponse.AccountName = accountChart?.AccountName ?? null;
        return _clearingReponse;
    }

    /// <summary>
    /// Search
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<AccountClearingSearchReponseModel>> Search(
        SimpleSearchModel model
    )
    {
        var branchs = await _branchService.GetBranchByBranchName("");
        var clearingBranchs = branchs;

        var query = (
            from a in _accountClearingRepository.Table
            join b in branchs.Branchs on a.BranchCode equals b.BranchCode into branchtmp
            from _branchtmp in branchtmp.DefaultIfEmpty()
            join c in clearingBranchs.Branchs
                on a.ClearingBranchCode equals c.BranchCode
                into clearingtmp
            from _clearingtmp in clearingtmp.DefaultIfEmpty()
            join d in _accountChartService.Table
                on a.AccountNumber equals d.AccountNumber
                into chartTmp
            from _chartTmp in chartTmp.DefaultIfEmpty()
            where
                !string.IsNullOrEmpty(model.SearchText)
                    ? (
                        _branchtmp.BranchName.Equals(model.SearchText)
                        || _clearingtmp.BranchName.Equals(model.SearchText)
                        || a.CurrencyId == model.SearchText
                        || a.ClearingType.Contains(model.SearchText, Constants.ICIC)
                        || a.AccountNumber.Contains(model.SearchText, Constants.ICIC)
                    )
                    : true
            select new AccountClearingSearchReponseModel
            {
                Id = a.Id,
                BranchCode = a.BranchCode,
                BranchName = _branchtmp == null ? string.Empty : _branchtmp.BranchName,
                CurrencyId = a.CurrencyId,
                ClearingBranchCode = _clearingtmp == null ? string.Empty : _clearingtmp.BranchCode,
                ClearingBranchName = _clearingtmp == null ? string.Empty : _clearingtmp.BranchName,
                ClearingType = a.ClearingType,
                AccountNumber = a.AccountNumber,
            }
        ).ToList();

        var _clearing = await query.AsQueryable().ToPagedList(model.PageIndex, model.PageSize);
        return _clearing;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual async Task<IPagedList<AccountClearingSearchReponseModel>> Search(
        AccountClearingSearchModel model
    )
    {
        var branchs = await _branchService.GetBranchByBranchName("");
        var clearingBranchs = branchs;
        var clearing = await _accountClearingRepository.GetAll(query =>
        {
            if (!string.IsNullOrEmpty(model.CurrencyId))
            {
                query = query.Where(c => c.CurrencyId == model.CurrencyId);
            }

            if (!string.IsNullOrEmpty(model.ClearingType))
            {
                query = query.Where(c => c.ClearingType == model.ClearingType);
            }

            if (!string.IsNullOrEmpty(model.AccountNumber))
            {
                query = query.Where(c =>
                    c.AccountNumber.Contains(model.AccountNumber, Constants.ICIC)
                );
            }

            if (!string.IsNullOrEmpty(model.BranchName))
            {
                query = query.Where(c => c.BranchCode.Equals(model.BranchName));
            }

            if (!string.IsNullOrEmpty(model.ClearingBranchName))
            {
                query = query.Where(c => c.ClearingBranchCode.Equals(model.ClearingBranchName));
            }

            return query;
        });
        var query = (
            from a in clearing
            from b in branchs.Branchs.Where(x => x.BranchCode == a.BranchCode)
            from c in clearingBranchs.Branchs.Where(x => x.BranchCode == a.ClearingBranchCode)
            select new AccountClearingSearchReponseModel
            {
                Id = a.Id,
                BranchCode = a?.BranchCode,
                BranchName = b?.BranchName,
                CurrencyId = a?.CurrencyId,
                ClearingBranchCode = a?.BranchCode,
                ClearingBranchName = c?.BranchName,
                ClearingType = a?.ClearingType,
                AccountNumber = a?.AccountNumber,
            }
        ).ToList();

        var _clearing = await query.AsQueryable().ToPagedList(model.PageIndex, model.PageSize);
        return _clearing;
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="clearing"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    public virtual async Task<AccountClearingCRUDReponseModel> Create(
        AccountClearing clearing,
        string referenceId = ""
    )
    {
        if (
            await IsUniqueKeyExist(
                clearing.BranchCode,
                clearing.CurrencyId,
                clearing.ClearingBranchCode,
                clearing.ClearingType
            )
        )
        {
            throw new O24OpenAPIException(
                "[Branch name - Currency Code - Clearing Branch Code - Clearing Type] is unique"
            );
        }

        var branchOrginal = await _branchService.GetBranchByBranchCode(clearing.BranchCode);
        if (branchOrginal.Id == 0)
        {
            throw new O24OpenAPIException("Branch not exist");
        }

        var clearingBranch = await _branchService.GetBranchByBranchCode(
            clearing.ClearingBranchCode
        );
        if (clearingBranch.Id == 0)
        {
            throw new O24OpenAPIException("Branch not exist");
        }

        await clearing.Insert(referenceId);

        var _clearingReponse = clearing.ToAccountClearingCRUDReponseModel();
        _clearingReponse.BranchName = branchOrginal.BranchName;
        _clearingReponse.ClearingBranchName = clearingBranch.BranchName;

        return _clearingReponse;
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="clearing"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    public virtual async Task Update(AccountClearing clearing, string referenceId = "")
    {
        if (clearing == null || clearing.Id == 0)
        {
            throw new ArgumentNullException("Clearing account not found");
        }

        var check = await GetByUniqueKey(
            clearing.BranchCode,
            clearing.CurrencyId,
            clearing.ClearingBranchCode,
            clearing.ClearingType
        );
        if (check != null && check.Id != clearing.Id)
        {
            throw new O24OpenAPIException(
                "[Branch name - Currency Code - Clearing Branch Code - Clearing Type] is unique"
            );
        }

        await _accountClearingRepository.Update(clearing, referenceId);
    }

    /// <summary>
    /// GetByUniqueKey
    /// </summary>
    /// <param name="branchCode"></param>
    /// <param name="currencyId"></param>
    /// <param name="clearingBranchCode"></param>
    /// <param name="clearingType"></param>
    /// <returns></returns>
    public virtual async Task<AccountClearing> GetByUniqueKey(
        string branchCode,
        string currencyId,
        string clearingBranchCode,
        string clearingType
    )
    {
        return await _accountClearingRepository
            .Table.Where(c =>
                c.BranchCode.Equals(branchCode)
                && c.CurrencyId.Equals(currencyId)
                && c.ClearingBranchCode.Equals(clearingBranchCode)
                && c.ClearingType.Equals(clearingType)
            )
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// IsUniqueKeyExist
    /// </summary>
    /// <param name="branchCode"></param>
    /// <param name="currencyId"></param>
    /// <param name="clearingBranchCode"></param>
    /// <param name="clearingType"></param>
    /// <returns></returns>
    public virtual async Task<bool> IsUniqueKeyExist(
        string branchCode,
        string currencyId,
        string clearingBranchCode,
        string clearingType
    )
    {
        var account = await GetByUniqueKey(
            branchCode,
            currencyId,
            clearingBranchCode,
            clearingType
        );
        return (account != null);
    }

    /// <summary>
    ///
    /// </summary>
    public virtual IQueryable<AccountClearing> Table => _accountClearingRepository.Table;
}
