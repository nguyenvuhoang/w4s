using O24OpenAPI.Core;
using O24OpenAPI.Core.Extensions;
using O24OpenAPI.Framework.Exceptions;
using O24OpenAPI.O24ACT.Common;
using O24OpenAPI.O24ACT.Config;
using O24OpenAPI.O24ACT.Models;
using O24OpenAPI.O24ACT.Services.Interfaces;

namespace O24OpenAPI.O24ACT.Services;

public class ProccessTransRefActService(
    AccountingSettings accountingSettings,
    IAccountClearingService accountClearingService,
    IForeignExchangeAccountDefinitionService foreignExchangeAccountDefinitionService,
    IAccountChartService accountChartService
) : IProccessTransRefActService
{
    private readonly AccountingSettings _accountingSeting = accountingSettings;
    private readonly IAccountClearingService _accountClearingService = accountClearingService;
    private readonly IForeignExchangeAccountDefinitionService _foreignExchangeAccountDefinitionService =
        foreignExchangeAccountDefinitionService;
    private readonly IAccountChartService _accountChartService = accountChartService;

    public virtual async Task<AccountingRuleProccessItemModel> AddFXAccount(
        int accountingEntryGroup,
        int accountingEntryIndex,
        string debitOrCredit,
        decimal amount,
        string branch,
        string accountCurrencyId,
        string clearingCurrencyId,
        string baseAmount,
        string fxClearingType,
        string accountName,
        string referenceId = ""
    )
    {
        var result = new AccountingRuleProccessItemModel();
        var accountfx = await _foreignExchangeAccountDefinitionService.GetByUniqueKey(
            branch,
            accountCurrencyId,
            clearingCurrencyId,
            fxClearingType
        );

        if (accountfx == null)
        {
            throw new NotFoundException(
                "ForeignExchangeAccountDefinition",
                $"{branch} - {accountCurrencyId} - {clearingCurrencyId} - {fxClearingType}"
            );
        }

        var accountChartInfor = await _accountChartService.GetByAccountNumberPostingProcess(
            accountfx.AccountNumber
        );
        if (accountChartInfor.AccountChart == null || accountChartInfor.AccountBalance == null)
        {
            accountChartInfor = await _accountChartService.OpenAccountPosting(
                accountfx.AccountNumber
            );
            if (accountChartInfor.AccountChart == null || accountChartInfor.AccountBalance == null)
            {
                throw new O24OpenAPIException(
                    "Accounting.ProccessTransRefActService.OpenAutoAccountFX"
                );
            }

            accountChartInfor.AccountChart.BalanceSide = Constants.BalanceSide.Both;
            accountChartInfor.AccountChart.PostingSide = Constants.PostingSide.Both;
            await _accountChartService.Update(accountChartInfor.AccountChart, referenceId);
        }
        var rule = new TemporaryPosting();
        rule.AccountName = accountChartInfor.AccountChart.AccountName;
        rule.AccountNumber = accountChartInfor.AccountChart.AccountNumber;
        rule.BranchGLBankAccountNumber = accountChartInfor.AccountChart.BranchCode;
        rule.CurrencyCodeGLBankAccountNumber = accountChartInfor.AccountChart.CurrencyCode;
        rule.CurrencyCode = accountChartInfor.AccountChart.CurrencyCode;
        rule.DebitOrCredit = debitOrCredit;
        rule.AccountingEntryGroup = accountingEntryGroup;
        rule.AccountingEntryIndex = accountingEntryIndex;
        rule.BaseAmount = baseAmount;
        rule.OriginalAmount = amount;
        rule.Amount = amount;
        rule.AccountLevel = accountChartInfor.AccountChart.AccountLevel;
        rule.BalanceSide = accountChartInfor.AccountChart.BalanceSide;
        if (accountName.Contains("EQUIVALENT"))
        {
            rule.ClearingType = "E";
        }
        else if (accountName.Contains("POSITION"))
        {
            rule.ClearingType = "P";
        }

        result.EntryJournal = rule;
        result.AccountChart = accountChartInfor.AccountChart;

        return result;
    }

    public virtual async Task<AccountingRuleProccessModel> ProcessFX_IBT(
        AccountingRuleProccessModel model,
        string hostBranch,
        string referenceId = ""
    )
    {
        string baseCurrency = _accountingSeting.CurrentBaseCurrency;

        var processGLGroup = new TemporaryGroupPostings(model.EntryJournals);
        processGLGroup.BaseCurrency = baseCurrency;
        processGLGroup.HostBranch = hostBranch;
        processGLGroup.IsIbtViaHo = _accountingSeting.IbtViaHo;

        processGLGroup.SortItems();
        processGLGroup.ExpandItems();

        string vacname = string.Empty;
        string vadded_br = string.Empty;
        string vadded_br1 = string.Empty;
        string vadded_br2 = string.Empty;

        foreach (var g in processGLGroup.TemporaryGroups)
        {
            if (g.HasFX)
            {
                int _vmdl_count = 1;
                int _acdix1 = -1;
                int _acdix2 = -1;
                string vacno1 = string.Empty;
                string vbaseamounttag = string.Empty;
                var listRules = (
                    from a in g.Postings
                    group a by (
                        a.AccountingEntryGroup,
                        a.AccountNumber,
                        a.CurrencyCodeGLBankAccountNumber
                    ) into r
                    select new
                    {
                        AccountingEntryGroup = r.Key.AccountingEntryGroup,
                        GLAccountNumber = r.Key.AccountNumber,
                        AccountingEntryIndex = r.Min(q => q.AccountingEntryIndex),
                        CurrencyCodeGLBankAccountNumber = r.Key.CurrencyCodeGLBankAccountNumber,
                        BaseAmount = r.Max(q => q.BaseAmount),
                    }
                ).ToList();
                foreach (var item in listRules)
                {
                    if (_vmdl_count == 1)
                    {
                        _acdix1 = item.AccountingEntryIndex;
                    }
                    else
                    {
                        _acdix2 = item.AccountingEntryIndex;
                    }
                    vbaseamounttag = item.BaseAmount;
                    _vmdl_count = _vmdl_count + 1;
                }

                if (_acdix1 != -1 && _acdix2 != -1)
                {
                    vacname = "FX";
                    // process_ibt_by_cases
                    var account1 = (
                        from a in g.Postings
                        where a.AccountingEntryIndex >= _acdix1 && a.AccountingEntryIndex < _acdix2
                        group a by (
                            a.AccountNumber,
                            a.BranchGLBankAccountNumber,
                            a.CurrencyCodeGLBankAccountNumber,
                            a.DebitOrCredit
                        ) into r
                        select new OpenFXAccountModel
                        {
                            DebitOrCredit = r.Key.DebitOrCredit,
                            BranchGLBankAccountNumber = r.Key.BranchGLBankAccountNumber,
                            CurrencyCodeGLBankAccountNumber = r.Key.CurrencyCodeGLBankAccountNumber,
                            Amount = r.Sum(q => q.Amount),
                            AccountNumber = string.Join(
                                ",",
                                (
                                    from b in g.Postings
                                    where
                                        b.AccountingEntryIndex >= _acdix1
                                        && b.AccountingEntryIndex < _acdix2
                                    select b.AccountNumber
                                )
                            ),
                        }
                    ).FirstOrDefault();
                    var account2 = (
                        from a in g.Postings
                        where a.AccountingEntryIndex >= _acdix2
                        group a by (
                            a.AccountNumber,
                            a.BranchGLBankAccountNumber,
                            a.CurrencyCodeGLBankAccountNumber,
                            a.DebitOrCredit
                        ) into r
                        select new OpenFXAccountModel
                        {
                            DebitOrCredit = r.Key.DebitOrCredit,
                            BranchGLBankAccountNumber = r.Key.BranchGLBankAccountNumber,
                            CurrencyCodeGLBankAccountNumber = r.Key.CurrencyCodeGLBankAccountNumber,
                            Amount = r.Sum(q => q.Amount),
                            AccountNumber = string.Join(
                                ",",
                                (
                                    from b in g.Postings
                                    where b.AccountingEntryIndex >= _acdix2
                                    select b.AccountNumber
                                )
                            ),
                        }
                    ).FirstOrDefault();
                    decimal vbamount = 0;
                    if (!string.IsNullOrEmpty(vbaseamounttag))
                    {
                        vbamount = decimal.Parse(vbaseamounttag).Round(2);
                    }
                    else
                    {
                        throw new O24OpenAPIException(
                            "Accounting.ProccessTransRefActService.BaseAmountRequired"
                        );
                    }

                    if (account1 == null || account2 == null)
                    {
                        throw new O24OpenAPIException("Invalid Operation Exception");
                    }

                    if (
                        account1.CurrencyCodeGLBankAccountNumber == baseCurrency
                        || account2.CurrencyCodeGLBankAccountNumber == baseCurrency
                    )
                    {
                        if (_accountingSeting.IbtViaHo)
                        {
                            if (!_accountingSeting.FX_IBT)
                            {
                                vadded_br =
                                    account1.BranchGLBankAccountNumber == hostBranch
                                        ? account2.BranchGLBankAccountNumber
                                        : account1.BranchGLBankAccountNumber;
                            }
                            else
                            {
                                vadded_br =
                                    account1.BranchGLBankAccountNumber
                                    == account2.BranchGLBankAccountNumber
                                        ? account1.BranchGLBankAccountNumber
                                        : hostBranch;
                            }

                            var accountingItem = await AddFXAccount(
                                g.GroupIndex,
                                _acdix1 + 1,
                                account2.DebitOrCredit,
                                account1.Amount,
                                vadded_br,
                                account1.CurrencyCodeGLBankAccountNumber,
                                account2.CurrencyCodeGLBankAccountNumber,
                                vbaseamounttag,
                                "I",
                                $"{vacname}-POSITION-{vadded_br}",
                                referenceId
                            );
                            g.Postings.Add(accountingItem.EntryJournal);

                            g.Postings = g
                                .Postings.Select(c =>
                                {
                                    c.AccountingEntryGroup =
                                        (
                                            c.AccountingEntryGroup == g.GroupIndex
                                            && c.AccountingEntryIndex >= _acdix2
                                        )
                                            ? c.AccountingEntryGroup + 1
                                            : c.AccountingEntryGroup;
                                    return c;
                                })
                                .ToList();

                            var accountingItem2 = await AddFXAccount(
                                g.GroupIndex + 1,
                                _acdix2 - 1,
                                account1.DebitOrCredit,
                                account2.Amount,
                                vadded_br,
                                account2.CurrencyCodeGLBankAccountNumber,
                                account1.CurrencyCodeGLBankAccountNumber,
                                vbaseamounttag,
                                "I",
                                $"{vacname}-EQUIVALENT-{vadded_br}",
                                referenceId
                            );
                            g.Postings.Add(accountingItem2.EntryJournal);
                        }
                        else
                        {
                            var newGroup = new TemporaryGroupPosting
                            {
                                GroupIndex = g.GroupIndex - 1,
                                BaseCurrency = processGLGroup.BaseCurrency,
                                HostBranch = processGLGroup.HostBranch,
                            };
                            processGLGroup.ProcessingGroups.Add(newGroup);

                            var accountingItem = await AddFXAccount(
                                g.GroupIndex,
                                _acdix2 - 1,
                                account2.DebitOrCredit,
                                account1.Amount,
                                account1.BranchGLBankAccountNumber,
                                account1.CurrencyCodeGLBankAccountNumber,
                                account2.CurrencyCodeGLBankAccountNumber,
                                vbaseamounttag,
                                "I",
                                $"{vacname}-POSITION-{vadded_br}",
                                referenceId
                            );
                            g.Postings.Add(accountingItem.EntryJournal);

                            // g.Postings = g.Postings.Select(c => { c.AccountingEntryGroup = (c.AccountingEntryGroup == g.GroupIndex && c.AccountingEntryIndex >= _acdix2) ? c.AccountingEntryGroup + 1 : c.AccountingEntryGroup; return c; }).ToList();

                            var accountingItem2 = await AddFXAccount(
                                g.GroupIndex - 1,
                                _acdix1 + 1,
                                account1.DebitOrCredit,
                                account2.Amount,
                                account1.BranchGLBankAccountNumber,
                                account2.CurrencyCodeGLBankAccountNumber,
                                account1.CurrencyCodeGLBankAccountNumber,
                                vbaseamounttag,
                                "I",
                                $"{vacname}-EQUIVALENT-{vadded_br}",
                                referenceId
                            );
                            newGroup.Postings.Add(accountingItem2.EntryJournal);

                            var acc2 = g
                                .Postings.Where(i =>
                                    i.AccountingEntryIndex == _acdix2 && i.ClearingType == "O"
                                )
                                .First();

                            processGLGroup.MoveItemToGroupOf(acc2, newGroup);
                        }
                    }
                    else
                    {
                        if (!_accountingSeting.FX_IBT)
                        {
                            if (account1.BranchGLBankAccountNumber == hostBranch)
                            {
                                vadded_br1 = account2.BranchGLBankAccountNumber;
                                vadded_br2 = account2.BranchGLBankAccountNumber;
                            }
                            else
                            {
                                if (account2.BranchGLBankAccountNumber == hostBranch)
                                {
                                    vadded_br1 = account1.BranchGLBankAccountNumber;
                                    vadded_br2 = account1.BranchGLBankAccountNumber;
                                }
                                else
                                {
                                    vadded_br1 = account1.BranchGLBankAccountNumber;
                                    vadded_br2 = account2.BranchGLBankAccountNumber;
                                }
                            }
                        }
                        else
                        {
                            if (
                                account1.BranchGLBankAccountNumber
                                != account2.BranchGLBankAccountNumber
                            )
                            {
                                if (_accountingSeting.IbtViaHo)
                                {
                                    vadded_br1 = hostBranch;
                                    vadded_br2 = hostBranch;
                                }
                                else
                                {
                                    vadded_br1 = account1.BranchGLBankAccountNumber;
                                    vadded_br2 = account2.BranchGLBankAccountNumber;
                                }

                                g.IgnoreIBT = true;
                            }
                            else
                            {
                                vadded_br1 = account1.BranchGLBankAccountNumber;
                                vadded_br2 = account2.BranchGLBankAccountNumber;
                            }
                        }

                        var accountingItem = await AddFXAccount(
                            g.GroupIndex,
                            _acdix1 + 1,
                            account2.DebitOrCredit,
                            account1.Amount,
                            vadded_br1,
                            account1.CurrencyCodeGLBankAccountNumber,
                            baseCurrency,
                            vbaseamounttag,
                            "I",
                            $"{vacname}-POSITION-{account1.CurrencyCodeGLBankAccountNumber}",
                            referenceId
                        );
                        g.Postings.Add(accountingItem.EntryJournal);

                        var newGroup1 = new TemporaryGroupPosting
                        {
                            GroupIndex = g.GroupIndex + 1,
                            BaseCurrency = processGLGroup.BaseCurrency,
                            HostBranch = processGLGroup.HostBranch,
                        };
                        processGLGroup.ProcessingGroups.Add(newGroup1);

                        var accountingItem2 = await AddFXAccount(
                            g.GroupIndex + 1,
                            1,
                            account1.DebitOrCredit,
                            vbamount,
                            vadded_br1,
                            baseCurrency,
                            account1.CurrencyCodeGLBankAccountNumber,
                            vbaseamounttag,
                            "I",
                            $"{vacname}-EQUIVALENT-{account1.CurrencyCodeGLBankAccountNumber}",
                            referenceId
                        );
                        newGroup1.Postings.Add(accountingItem2.EntryJournal);

                        var accountingItem3 = await AddFXAccount(
                            g.GroupIndex + 1,
                            2,
                            account2.DebitOrCredit,
                            vbamount,
                            vadded_br2,
                            baseCurrency,
                            account2.CurrencyCodeGLBankAccountNumber,
                            vbaseamounttag,
                            "I",
                            $"{vacname}-EQUIVALENT-{account2.CurrencyCodeGLBankAccountNumber}",
                            referenceId
                        );
                        newGroup1.Postings.Add(accountingItem3.EntryJournal);

                        var newGroup2 = new TemporaryGroupPosting
                        {
                            GroupIndex = g.GroupIndex + 2,
                            BaseCurrency = processGLGroup.BaseCurrency,
                            HostBranch = processGLGroup.HostBranch,
                        };
                        processGLGroup.ProcessingGroups.Add(newGroup2);

                        var accountingItem4 = await AddFXAccount(
                            g.GroupIndex + 2,
                            _acdix2 - 1,
                            account1.DebitOrCredit,
                            account2.Amount,
                            vadded_br2,
                            account2.CurrencyCodeGLBankAccountNumber,
                            baseCurrency,
                            vbaseamounttag,
                            "I",
                            $"{vacname}-POSITION-{account2.CurrencyCodeGLBankAccountNumber}",
                            referenceId
                        );
                        newGroup2.Postings.Add(accountingItem4.EntryJournal);

                        var acc2 = g
                            .Postings.Where(i =>
                                i.AccountingEntryIndex == _acdix2 && i.ClearingType == "O"
                            )
                            .First();
                        processGLGroup.MoveItemToGroupOf(acc2, newGroup2);

                        var nonIbtRules = g
                            .Postings.Where(r => !r.AccountName.StartsWith($"{vacname}-EQUIVALENT"))
                            .ToList();
                        var ibtRules = g
                            .Postings.Where(r => r.AccountName.StartsWith($"{vacname}-EQUIVALENT"))
                            .ToList();

                        nonIbtRules.AddRange(model.EntryJournals);

                        model.EntryJournals = nonIbtRules;
                    }
                }
            }
        }

        processGLGroup.SortItems();

        if (_accountingSeting.FX_IBT)
        {
            await processGLGroup.ProcessIBT();
        }

        processGLGroup.SortItems();

        model.EntryJournals = processGLGroup.Postings;

        processGLGroup.SortItems();
        return model;
    }

    /// <summary>
    /// Add FX Account
    /// </summary>
    /// <param name="accountingEntryGroup"></param>
    /// <param name="accountingEntryIndex"></param>
    /// <param name="debitOrCredit"></param>
    /// <param name="amount"></param>
    /// <param name="sourceBranch"></param>
    /// <param name="destBranch"></param>
    /// <param name="currencyId"></param>
    /// <param name="baseAmount"></param>
    /// <param name="IBTClearingType"></param>
    /// <param name="referenceId"></param>
    /// <returns></returns>
    /// <exception cref="NeptuneNotFoundException"></exception>
    /// <exception cref="O24OpenAPIException"></exception>
    public virtual async Task<AccountingRuleProccessItemModel> AddCLearingAccount(
        int accountingEntryGroup,
        int accountingEntryIndex,
        string debitOrCredit,
        decimal amount,
        string sourceBranch,
        string destBranch,
        string currencyId,
        string baseAmount,
        string IBTClearingType,
        string referenceId = ""
    )
    {
        var result = new AccountingRuleProccessItemModel();
        var accountClearing = await _accountClearingService.GetByUniqueKey(
            sourceBranch,
            currencyId,
            destBranch,
            IBTClearingType
        );

        if (accountClearing == null)
        {
            throw new NotFoundException(
                "AccountClearing",
                $"{sourceBranch} - {currencyId} - {destBranch} - {IBTClearingType}"
            );
        }

        var accountChartInfor = await _accountChartService.GetByAccountNumberPostingProcess(
            accountClearing.AccountNumber
        );
        if (accountChartInfor.AccountChart == null || accountChartInfor.AccountBalance == null)
        {
            accountChartInfor = await _accountChartService.OpenAccountPosting(
                accountClearing.AccountNumber
            );
            if (accountChartInfor.AccountChart == null || accountChartInfor.AccountBalance == null)
            {
                throw new O24OpenAPIException(
                    "Accounting.ProccessTransRefActService.OpenAutoAccountClearing"
                );
            }

            accountChartInfor.AccountChart.BalanceSide = Constants.BalanceSide.Both;
            accountChartInfor.AccountChart.PostingSide = Constants.PostingSide.Both;
            await _accountChartService.Update(accountChartInfor.AccountChart, referenceId);
        }
        var rule = new TemporaryPosting();
        rule.AccountName = accountChartInfor.AccountChart.AccountName;
        rule.AccountNumber = accountChartInfor.AccountChart.AccountNumber;
        rule.BranchGLBankAccountNumber = accountChartInfor.AccountChart.BranchCode;
        rule.CurrencyCodeGLBankAccountNumber = accountChartInfor.AccountChart.CurrencyCode;
        rule.AccountLevel = accountChartInfor.AccountChart.AccountLevel;
        rule.BalanceSide = accountChartInfor.AccountChart.BalanceSide;
        rule.CurrencyCode = currencyId;
        rule.DebitOrCredit = debitOrCredit;
        rule.AccountingEntryGroup = accountingEntryGroup;
        rule.AccountingEntryIndex = accountingEntryIndex;
        rule.BaseAmount = baseAmount;
        rule.OriginalAmount = amount;
        rule.Amount = amount;
        rule.ClearingType = IBTClearingType;
        result.EntryJournal = rule;
        result.AccountChart = accountChartInfor.AccountChart;

        return result;
    }
}
