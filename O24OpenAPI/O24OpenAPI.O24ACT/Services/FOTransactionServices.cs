using O24OpenAPI.Core.Extensions;
using O24OpenAPI.O24ACT.Models;
using O24OpenAPI.O24ACT.Models.Request;
using O24OpenAPI.O24ACT.Models.Response;
using O24OpenAPI.O24ACT.Services.Interfaces;
using O24OpenAPI.Web.Framework.Domain;
using O24OpenAPI.Web.Framework.Extensions;

namespace O24OpenAPI.O24ACT.Services;

public class FOTransactionServices(
    ITransactionRulesService transactionRulesService,
    IAccountChartService accountChartService,
    IEntryjounalDataService contextEntry
) : IFOTransactionServices
{
    private readonly ITransactionRulesService _transactionRulesService =
        transactionRulesService;
    private readonly IAccountChartService _accountChartService = accountChartService;
    private readonly IEntryjounalDataService _contextEntry = contextEntry;

    public async Task<EntryPostingReponse> ExcuteAccountingRuleAsync(
        ExcuteAccountingRuleModel model
    )
    {
        var lstEntry = new List<TemporaryPosting>();
        model.TransactionNumber = model.DigitalTransactionId;
        try
        {
            if (!model.IsReverse)
            {
                if (model.ListGLFromResponse?.Count > 0)
                {
                    foreach (var item in model.ListGLFromResponse)
                    {
                        if (!item.CurrencyCode.HasValue())
                        {
                            var accountChart = await _accountChartService.GetByAccountNumber(
                                item.GLAccount
                            );
                            item.CurrencyCode = accountChart.CurrencyCode;
                        }

                        model.ListGL.Add(
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
                if (model.ListGL.Count > 0)
                {
                    foreach (var item in model.ListGL.Where(x => x.Amount != 0))
                    {
                        string condition = System.Text.RegularExpressions.Regex.Unescape(
                            item.Condition ?? string.Empty
                        );
                        var result = true;
                        if (condition.HasValue()) { }
                        if (result)
                        {
                            if (!item.CurrencyCode.HasValue())
                            {
                                var accountChart =
                                    await _accountChartService.GetByAccountNumber(
                                        item.GLAccount
                                    );
                                item.CurrencyCode = accountChart.CurrencyCode;
                            }

                            model.Postings?.Add(item);
                        }
                    }
                }

                if (model.GLCommon.AccountCommon.HasValue())
                {
                    model.ListGLCommon.Add(model.GLCommon);
                }

                foreach (var item in model.ListGLCommon)
                {
                    var accgrpformat = await _contextEntry.GetGLAccountCommonFormat(
                        item.AccountCommon
                    );
                    var branchCode = item.BranchCode.HasValue()
                        ? item.BranchCode
                        : model.CurrentBranchCode;
                    var account = await _contextEntry.ChangeGLFormatFromUserCurrency(
                        accgrpformat,
                        branchCode,
                        item.CurrencyCode
                    );

                    if (!item.CurrencyCode.HasValue())
                    {
                        var accountChart = await _accountChartService.GetByAccountNumber(
                            account
                        );
                        item.CurrencyCode = accountChart.CurrencyCode;
                    }

                    model.Postings.Add(
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
            var reponse = await _contextEntry.ExcuteEntryPosting(model);
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
}
