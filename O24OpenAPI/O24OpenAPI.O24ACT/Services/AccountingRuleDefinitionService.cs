using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.O24ACT.Models;
using O24OpenAPI.O24ACT.Services.Interfaces;

namespace O24OpenAPI.O24ACT.Services;

public class AccountingRuleDefinitionService : IAccountingRuleDefinitionService
{
    public AccountingRuleDefinitionModel GeneratedPostingTemporory(BaseTransactionModel model)
    {
        var postings = model
            .Postings.Select(
                (p, index) =>
                    new TemporaryPosting
                    {
                        AccountNumber = p.GLAccount,
                        CurrencyCode = p.CurrencyCode,
                        Amount = p.Amount,
                        DebitOrCredit = p.DorC,
                        AccountingEntryGroup = p.AccountingGroup,
                        AccountingEntryIndex = index + 1,
                        BaseAmount = p.BaseCurrencyAmount.ToString(),
                        SysAccountName = p.SysAccountName,
                        PostGL = !p.Posted,
                        GroupOfSendingTemplate = p.AccountingGroup,
                        TransId = p.TransId,
                        TransTableName = p.TransTableName,
                    }
            )
            .ToList();

        var reponse = new AccountingRuleDefinitionModel();
        if (postings.Count == 0)
        {
            return reponse;
        }

        reponse.EntryJournals = postings;

        return reponse;
    }
}
