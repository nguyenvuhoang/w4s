using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.O24ACT.Services.Interfaces;

namespace O24OpenAPI.O24ACT.Models;

public class TemporaryGroupPostings : BaseO24OpenAPIModel
{
    public TemporaryGroupPostings(List<TemporaryPosting> postings)
    {
        if (postings is null || !postings.Any())
        {
            return;
        }

        var gs = postings
            .OrderBy(p => p.AccountingEntryGroup)
            .Select(p => p.AccountingEntryGroup)
            .Distinct()
            .ToList();

        foreach (var g in gs)
        {
            var groups = new TemporaryGroupPosting
            {
                GroupIndex = g,
                BaseCurrency = BaseCurrency,
                HostBranch = HostBranch,
            };
            var ps = postings.Where(p => p.AccountingEntryGroup == g).ToList();
            groups.Postings.AddRange(ps);

            TemporaryGroups.Add(groups);
        }
    }

    public void SortItems()
    {
        TemporaryGroups.AddRange(ProcessingGroups);
        var gs = TemporaryGroups
            .Where(g => g.Postings.Count > 0)
            .OrderBy(g => g.GroupIndex)
            .ToList();
        var index = 1;
        foreach (var g in gs)
        {
            g.GroupIndex = index;
            g.BaseCurrency = BaseCurrency;
            g.HostBranch = HostBranch;
            g.SortItems();
            index++;
        }
        TemporaryGroups = gs;
        ProcessingGroups = new();
    }

    public string HostBranch { get; set; }

    public string BaseCurrency { get; set; }
    public bool IsIbtViaHo { get; set; }

    public List<TemporaryGroupPosting> TemporaryGroups { get; set; } = [];
    public List<TemporaryGroupPosting> ProcessingGroups { get; set; } = new();

    public void ExpandItems()
    {
        if (!TemporaryGroups.Any())
        {
            return;
        }

        TemporaryGroups.ForEach(g => g.ExpandItems());
    }

    public List<TemporaryPosting> Postings
    {
        get
        {
            var postings = new List<TemporaryPosting>();
            foreach (var g in TemporaryGroups)
            {
                postings.AddRange(g.Postings);
            }
            return postings;
        }
    }

    public void MoveItemToGroupOf(TemporaryPosting item, TemporaryPosting destItem)
    {
        var sourceGroup = TemporaryGroups
            .Where(g => g.GroupIndex == item.AccountingEntryGroup)
            .FirstOrDefault();
        sourceGroup ??= new();

        foreach (var i in sourceGroup.Postings)
        {
            if (
                i.BranchGLBankAccountNumber == item.BranchGLBankAccountNumber
                && i.DebitOrCredit == item.DebitOrCredit
                && i.AccountingEntryGroup == item.AccountingEntryGroup
                && i.AccountingEntryIndex == item.AccountingEntryIndex
            )
            {
                sourceGroup.Postings.Remove(i);
                break;
            }
        }

        var destGroup = TemporaryGroups
            .Where(g => g.GroupIndex == destItem.AccountingEntryGroup)
            .FirstOrDefault();
        destGroup ??= new();
        item.AccountingEntryGroup = destItem.AccountingEntryGroup;
        destGroup.Postings.Add(item);
    }

    public void MoveItemToGroupOf(TemporaryPosting item, TemporaryGroupPosting destGroup)
    {
        var sourceGroup = TemporaryGroups
            .Where(g => g.GroupIndex == item.AccountingEntryGroup)
            .FirstOrDefault();
        sourceGroup ??= new();

        foreach (var i in sourceGroup.Postings)
        {
            if (
                i.BranchGLBankAccountNumber == item.BranchGLBankAccountNumber
                && i.DebitOrCredit == item.DebitOrCredit
                && i.AccountingEntryGroup == item.AccountingEntryGroup
                && i.AccountingEntryIndex == item.AccountingEntryIndex
            )
            {
                sourceGroup.Postings.Remove(i);
                break;
            }
        }

        item.AccountingEntryGroup = destGroup.GroupIndex;
        destGroup.Postings.Add(item);
    }

    public async Task ProcessIBT()
    {
        ExpandItems();
        var clearingType = "I";

        var service = EngineContext.Current.Resolve<IProccessTransRefActService>();
        foreach (var group in TemporaryGroups.Where(g => g.HasIBT && !g.IgnoreIBT).ToList())
        {
            if (group.HasIBT)
            {
                var groupRules = new List<TemporaryPosting>();
                var newGroup = new TemporaryGroupPosting()
                {
                    GroupIndex = group.GroupIndex - 1,
                    BaseCurrency = BaseCurrency,
                    HostBranch = HostBranch,
                };
                ProcessingGroups.Add(newGroup);

                if (IsIbtViaHo)
                {
                    var rulesProcessesNoHo = group
                        .Postings.Where(q => q.BranchGLBankAccountNumber != HostBranch)
                        .ToList();
                    foreach (var ruleProcess in rulesProcessesNoHo)
                    {
                        groupRules.Add(
                            (
                                await service.AddCLearingAccount(
                                    group.GroupIndex,
                                    ruleProcess.AccountingEntryIndex + 1,
                                    ruleProcess.DebitOrCredit == "D" ? "C" : "D",
                                    ruleProcess.Amount,
                                    ruleProcess.BranchGLBankAccountNumber,
                                    HostBranch,
                                    ruleProcess.CurrencyCodeGLBankAccountNumber,
                                    ruleProcess.BaseAmount,
                                    clearingType
                                )
                            )?.EntryJournal
                        );

                        groupRules.Add(
                            (
                                await service.AddCLearingAccount(
                                    group.GroupIndex,
                                    ruleProcess.AccountingEntryIndex + 1,
                                    ruleProcess.DebitOrCredit,
                                    ruleProcess.Amount,
                                    HostBranch,
                                    ruleProcess.BranchGLBankAccountNumber,
                                    ruleProcess.CurrencyCodeGLBankAccountNumber,
                                    ruleProcess.BaseAmount,
                                    clearingType
                                )
                            )?.EntryJournal
                        );
                    }
                }
                else
                {
                    var isMove = false;
                    var branch1 = group.Branch1;
                    var rulesProcesses = group
                        .Postings.Where(q => q.BranchGLBankAccountNumber != branch1)
                        .ToList();

                    foreach (var ruleProcess in rulesProcesses)
                    {
                        groupRules.Add(
                            (
                                await service.AddCLearingAccount(
                                    group.GroupIndex,
                                    ruleProcess.AccountingEntryIndex + 1,
                                    ruleProcess.DebitOrCredit == "D" ? "C" : "D",
                                    ruleProcess.Amount,
                                    ruleProcess.BranchGLBankAccountNumber,
                                    branch1,
                                    ruleProcess.CurrencyCodeGLBankAccountNumber,
                                    ruleProcess.BaseAmount,
                                    clearingType
                                )
                            )?.EntryJournal
                        );

                        if (isMove)
                        {
                            continue;
                        }

                        var otherBranchRules = group
                            .Postings.Where(q => q.BranchGLBankAccountNumber == branch1)
                            .ToList();
                        foreach (var ruleOtherProcess in otherBranchRules)
                        {
                            newGroup.Postings.Add(
                                (
                                    await service.AddCLearingAccount(
                                        newGroup.GroupIndex,
                                        ruleOtherProcess.AccountingEntryIndex + 1,
                                        ruleOtherProcess.DebitOrCredit == "D" ? "C" : "D",
                                        ruleOtherProcess.Amount,
                                        branch1,
                                        ruleProcess.BranchGLBankAccountNumber,
                                        ruleOtherProcess.CurrencyCodeGLBankAccountNumber,
                                        ruleOtherProcess.BaseAmount,
                                        clearingType
                                    )
                                )?.EntryJournal
                            );

                            MoveItemToGroupOf(ruleOtherProcess, newGroup);
                        }
                        isMove = true;
                    }
                }
                group.Postings?.AddRange(groupRules);
            }
        }
    }
}
