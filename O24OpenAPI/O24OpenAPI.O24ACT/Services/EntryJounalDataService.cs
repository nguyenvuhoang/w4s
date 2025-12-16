using LinqToDB;
using Newtonsoft.Json;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.O24ACT.Common;
using O24OpenAPI.O24ACT.Configuration;
using O24OpenAPI.O24ACT.Domain;
using O24OpenAPI.O24ACT.Models;
using O24OpenAPI.O24ACT.Models.Response;
using O24OpenAPI.O24ACT.Services.Interfaces;
using static O24OpenAPI.O24ACT.Common.Constants;

namespace O24OpenAPI.O24ACT.Services;

public class EntryJounalDataService(
    IAccountCommonService accountCommonService,
    AccountingSettings accountingSeting,
    ICurrencyService currencyService,
    IAccountChartService accountChartService,
    IProccessTransRefActService proccessTransRefActService,
    IAccountingRuleDefinitionService actDefinitionRuleService,
    IAccountBalanceService accountBalanceService,
    ITransactionActionService transactionActionService,
    IRepository<AccountBalance> accountBalanceRepo
) : IEntryjounalDataService
{
    private readonly IAccountCommonService _accountCommonService = accountCommonService;
    private readonly AccountingSettings _accountingSeting = accountingSeting;
    private readonly ICurrencyService _currencyService = currencyService;
    private readonly IAccountingRuleDefinitionService _actDefinitionRuleService =
        actDefinitionRuleService;
    private readonly IAccountChartService _accountChartService = accountChartService;
    private readonly IProccessTransRefActService _proccessTransRefActService =
        proccessTransRefActService;
    private readonly IAccountBalanceService _accountBalanceService = accountBalanceService;
    private readonly ITransactionActionService _transactionActionService = transactionActionService;
    private readonly IRepository<AccountBalance> _accountBalanceRepo = accountBalanceRepo;

    public async Task<string> GetGLAccountCommonFormat(string accountName, string language = "en")
    {
        var check = await _accountCommonService.GetByAccountName(accountName.Trim());
        if (check == null || check.Id == 0)
        {
            throw await O24Exception.CreateAsync(
                O24ActResourceCode.Validation.AccountCommonNotDefined,
                language,
                [accountName]
            );
        }

        return check.AccountNumber;
    }

    public virtual async Task<string> ChangeGLFormatFromUserCurrency(
        string accountNumberFormat,
        string branchCode,
        string currencyCode
    )
    {
        int branchLengh = _accountingSeting.LengthBranch;

        accountNumberFormat =
            (!accountNumberFormat.Contains("#".PadLeft(branchLengh, '#')))
                ? accountNumberFormat
                : accountNumberFormat.Replace("#".PadLeft(branchLengh, '#'), branchCode);
        accountNumberFormat =
            (!accountNumberFormat.Contains("-".PadLeft(branchLengh, '-')))
                ? accountNumberFormat
                : accountNumberFormat.Replace("-".PadLeft(branchLengh, '-'), branchCode);
        if (accountNumberFormat.Contains("**") || accountNumberFormat.Contains('?'))
        {
            var currency = await _currencyService.GetCurrency(currencyCode);
            accountNumberFormat =
                (!accountNumberFormat.Contains("**"))
                    ? accountNumberFormat
                    : accountNumberFormat.Replace("**", currency.ShortCurrencyId);
            accountNumberFormat =
                (!accountNumberFormat.Contains('?'))
                    ? accountNumberFormat
                    : accountNumberFormat.Replace(
                        "?",
                        int.Parse(currency.ShortCurrencyId) < 9
                            ? int.Parse(currency.ShortCurrencyId).ToString()
                            : "9"
                    );
        }
        return accountNumberFormat;
    }

    /// <summary>
    /// Get Account Chart Entry Journal
    /// </summary>
    /// <param name="rules"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    public virtual async Task<AccountingRuleProccessModel> GetAccountChartEntryJournal(
        List<TemporaryPosting> rules,
        string language = "en"
    )
    {
        var _process = new AccountingRuleProccessModel();

        foreach (var item in rules)
        {
            var accountchartInforItem = await _accountChartService.GetByAccountNumber(
                item.AccountNumber
            );
            if (accountchartInforItem == null || accountchartInforItem.Id == 0)
            {
                await _accountChartService.OpenAccountPosting(
                    item.AccountNumber,
                    null,
                    null,
                    "",
                    item.TransId
                );
                accountchartInforItem = await _accountChartService.GetByAccountNumber(
                    item.AccountNumber
                );
            }

            item.AccountNumber = accountchartInforItem.AccountNumber;
            if (item.AccountName == "CASH" || accountchartInforItem.AccountGroup.Equals("C"))
            {
                item.IsCashAccount = true;
            }

            item.AccountName = accountchartInforItem.AccountName;
            item.BalanceSide = accountchartInforItem.BalanceSide;
            item.AccountLevel = accountchartInforItem.AccountLevel;
            item.BranchGLBankAccountNumber = accountchartInforItem.BranchCode;
            item.CurrencyCodeGLBankAccountNumber = accountchartInforItem.CurrencyCode;
            _process.EntryJournals.Add(item);
            _process.Language = language;
        }
        return _process;
    }

    /// <summary>
    /// CheckTotalBalanceEntry
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public virtual bool CheckBalancePosting(List<TemporaryPosting> model)
    {
        decimal checkAmount = model.Sum(x =>
            x.DebitOrCredit == Constants.EntryAction.Debit ? -x.Amount : x.Amount
        );
        if (checkAmount != 0)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Execute Entry Posting
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    /// <exception cref="NeptuneException"></exception>
    public virtual async Task<EntryPostingReponse> ExcuteEntryPosting(BaseTransactionModel message)
    {
        var result = new EntryPostingReponse();

        var entryDefine = _actDefinitionRuleService.GeneratedPostingTemporory(message);

        entryDefine.EntryJournals = entryDefine
            .EntryJournals.Select(c =>
            {
                c.Amount = c.Amount.Round(2);
                return c;
            })
            .ToList();

        result.ErrorEntryJournals = entryDefine.EntryJournals;

        //do_auto_open_account
        var accountingProcess = await GetAccountChartEntryJournal(rules: entryDefine.EntryJournals);
        var hostBranchCode = _accountingSeting.HostBranchCode.HasValue()
            ? _accountingSeting.HostBranchCode
            : "0000";
        accountingProcess = await _proccessTransRefActService.ProcessFX_IBT(
            accountingProcess,
            hostBranchCode,
            message.RefId
        );

        result.ErrorEntryJournals = accountingProcess
            .EntryJournals.OrderBy(q => q.AccountingEntryGroup)
            .ThenBy(q => q.AccountingEntryIndex)
            .ToList();

        if (!CheckBalancePosting(accountingProcess.EntryJournals) && !message.IsReverse)
        {
            var error = JsonConvert.SerializeObject(accountingProcess.EntryJournals);
            Console.WriteLine(error);
            await error.LogInfoAsync();
            throw await O24Exception.CreateAsync(
                O24ActResourceCode.Validation.UnBalancePosting,
                message.Language
            );
        }
        result.ErrorCode = PostingErrorCode.Pending;
        await UpdatePosting(message, accountingProcess);
        result.ErrorCode = PostingErrorCode.Success;
        result.EntryJournals = accountingProcess
            .EntryJournals.OrderBy(x => x.AccountingEntryGroup)
            .ThenBy(x => x.AccountingEntryIndex)
            .ToList();
        return result;
    }

    public async Task UpdatePosting(
        BaseTransactionModel message,
        AccountingRuleProccessModel entryDefine
    )
    {
        // Check Account
        if (entryDefine.EntryJournals != null && entryDefine.EntryJournals.Count != 0)
        {
            var lst = JsonConvert.DeserializeObject<List<int>>(
                _accountingSeting?.AllowPostingWithLevel
            );

            // update master
            var acstmLines = entryDefine.EntryJournals.OrderBy(o => o.AccountNumber).ToList();
            foreach (var item in acstmLines)
            {
                if (_accountingSeting.AllowPostingWithLevel.HasValue())
                {
                    if (!lst.Contains(item.AccountLevel))
                    {
                        throw await O24Exception.CreateAsync(
                            O24ActResourceCode.Validation.UnBalancePosting,
                            message.Language,
                            [item.AccountLevel]
                        );
                    }
                }

                _accountBalanceRepo
                    .Table.Where(s => s.AccountNumber == item.AccountNumber)
                    .Set(a => a.AccountNumber, a => a.AccountNumber)
                    .Update();

                var accountBalance = await _accountBalanceService.GetByAccountNumber(
                    item.AccountNumber
                );

                if (
                    item.BalanceSide == BalanceSide.Credit
                    && item.DebitOrCredit == PostingSide.Debit
                    && accountBalance.Balance - item.Amount < 0
                )
                {
                    throw await O24Exception.CreateAsync(
                        O24ActResourceCode.Validation.ACT_NOT_ALLOW_DEBIT_BAL,
                        message.Language,
                        [item.AccountNumber]
                    );
                }
                else if (
                    item.BalanceSide == BalanceSide.Debit
                    && item.DebitOrCredit == PostingSide.Credit
                    && accountBalance.Balance + item.Amount > 0
                )
                {
                    throw await O24Exception.CreateAsync(
                        O24ActResourceCode.Validation.ACT_NOT_ALLOW_CREDIT_BAL,
                        message.Language,
                        [item.AccountNumber]
                    );
                }

                // Post GL

                var transId = "";
                if (item.DebitOrCredit == PostingSide.Debit)
                {
                    await _accountBalanceService.UpdateAccount(
                        message,
                        item.AccountNumber,
                        PostingSide.Debit,
                        item.Amount
                    );
                    transId = await accountBalance.ExecuteTran(
                        message,
                        PostingSide.Debit,
                        item.Amount,
                        statementCode: "WDR"
                    );
                }
                else
                {
                    await _accountBalanceService.UpdateAccount(
                        message,
                        item.AccountNumber,
                        PostingSide.Credit,
                        item.Amount
                    );
                    transId = await accountBalance.ExecuteTran(
                        message,
                        PostingSide.Credit,
                        item.Amount,
                        statementCode: "DEP"
                    );
                }

                await _transactionActionService.PostGL(
                    item.TransId.HasValue() ? item.TransId : transId,
                    item.TransTableName ?? string.Empty,
                    message.TransactionNumber,
                    message.ValueDate,
                    item.SysAccountName ?? string.Empty,
                    item.AccountNumber,
                    item.Amount,
                    item.DebitOrCredit,
                    item.BranchGLBankAccountNumber,
                    item.CurrencyCodeGLBankAccountNumber ?? string.Empty,
                    "",
                    "",
                    Convert.ToDecimal(item.BaseAmount),
                    message.IsReverse,
                    item.AccountingEntryGroup
                );
            }

            entryDefine.EntryJournals =
            [
                .. entryDefine
                    .EntryJournals.OrderBy(x => x.AccountingEntryGroup)
                    .ThenBy(x => x.AccountingEntryIndex),
            ];
        }
    }
}
