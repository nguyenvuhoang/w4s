using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.ACT.API.Application.Models;
using O24OpenAPI.ACT.API.Application.Models.Response;
using O24OpenAPI.ACT.API.Application.Services;
using O24OpenAPI.ACT.Domain.AggregatesModel.AccountAggregate;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core.Caching;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Domain;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ACT.API.Application.Features.FOTransactions;

public class ExcuteAccountingRuleCommand : BaseTransactionModel, ICommand<EntryPostingReponse>
{
    public List<GLEntriesModel> ListGL { get; set; }
    public List<GLEntriesFromResponseModel> ListGLFromResponse { get; set; }
    public GLCommonModel GLCommon { get; set; }
    public List<GLCommonModel> ListGLCommon { get; set; }
    public dynamic Fields { get; set; }
    public string DigitalTransactionId { get; set; }
}

[CqrsHandler]
public class ExcuteAccountingRuleHandle(
    IAccountChartRepository accountChartRepository,
    IStaticCacheManager staticCacheManager,
    IEntryjounalDataService entryjounalDataService
) : ICommandHandler<ExcuteAccountingRuleCommand, EntryPostingReponse>
{
    [WorkflowStep(WorkflowStepCode.ACT.WF_STEP_ACT_EXECUTE_POSTING)]
    public async Task<EntryPostingReponse> HandleAsync(
        ExcuteAccountingRuleCommand request,
        CancellationToken cancellationToken = default
    )
    {
        List<TemporaryPosting> lstEntry = [];
        request.TransactionNumber = request.DigitalTransactionId;
        try
        {
            if (!request.IsReverse)
            {
                if (request.ListGLFromResponse?.Count > 0)
                {
                    foreach (GLEntriesFromResponseModel item in request.ListGLFromResponse)
                    {
                        if (!item.CurrencyCode.HasValue())
                        {
                            AccountChart accountChart = await GetByAccountNumber(
                                item.GLAccount
                            );
                            item.CurrencyCode = accountChart.CurrencyCode;
                        }

                        request.ListGL.Add(
                            new GLEntriesModel
                            {
                                GLAccount = item.GLAccount,
                                Amount = item.Amount,
                                DorC = item.DorC,
                                AccountingGroup = item.AccountingGroup,
                                Condition = item.Condition,
                                BranchCode = item.BranchCode,
                                CurrencyCode = item.CurrencyCode,
                            }
                        );
                    }
                }
                if (request.ListGL.Count > 0)
                {
                    foreach (GLEntriesModel item in request.ListGL.Where(x => x.Amount != 0))
                    {
                        string condition = System.Text.RegularExpressions.Regex.Unescape(
                            item.Condition ?? string.Empty
                        );
                        bool result = true;
                        if (condition.HasValue()) { }
                        if (result)
                        {
                            if (!item.CurrencyCode.HasValue())
                            {
                                AccountChart accountChart = await GetByAccountNumber(
                                    item.GLAccount
                                );
                                item.CurrencyCode = accountChart.CurrencyCode;
                            }

                            request.Postings?.Add(item);
                        }
                    }
                }

                if (request.GLCommon.AccountCommon.HasValue())
                {
                    request.ListGLCommon.Add(request.GLCommon);
                }

                foreach (GLCommonModel item in request.ListGLCommon)
                {
                    string accgrpformat = await entryjounalDataService.GetGLAccountCommonFormat(
                        item.AccountCommon
                    );
                    string branchCode = item.BranchCode.HasValue()
                        ? item.BranchCode
                        : request.CurrentBranchCode;
                    string account = await entryjounalDataService.ChangeGLFormatFromUserCurrency(
                        accgrpformat,
                        branchCode,
                        item.CurrencyCode
                    );

                    if (!item.CurrencyCode.HasValue())
                    {
                        AccountChart accountChart = await GetByAccountNumber(account);
                        item.CurrencyCode = accountChart.CurrencyCode;
                    }

                    request.Postings.Add(
                        new GLEntries
                        {
                            GLAccount = account,
                            Amount = item.Amount,
                            DorC = item.DorC,
                            SysAccountName = item.AccountCommon,
                            BranchCode = branchCode,
                            CurrencyCode = item.CurrencyCode,
                            AccountingGroup = item.AccountingGroup,
                        }
                    );
                }
            }
            EntryPostingReponse reponse = await entryjounalDataService.ExcuteEntryPosting(
                request
            );
            lstEntry = reponse.EntryJournals;
            return reponse;
        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();
            return new EntryPostingReponse
            {
                ErrorMessage = ex.GetBaseException().Message,
                EntryJournals = lstEntry,
            };
        }
    }

    public virtual async Task<AccountChart> GetByAccountNumber(string acno)
    {
        CacheKey cacheKey = new(acno);
        AccountChart account = await staticCacheManager.Get(
            cacheKey,
            async () =>
            {
                return await accountChartRepository
                    .Table.Where(c => c.AccountNumber == acno)
                    .FirstOrDefaultAsync();
            }
        );
        return account;
    }
}
