using O24OpenAPI.O24ACT.Models;

namespace O24OpenAPI.O24ACT.Services.Interfaces;

public interface IProccessTransRefActService
{
    Task<AccountingRuleProccessModel> ProcessFX_IBT(AccountingRuleProccessModel model, string hostBranch, string referenceId = "");
    Task<AccountingRuleProccessItemModel> AddCLearingAccount(int accountingEntryGroup, int accountingEntryIndex, string debitOrCredit, decimal amount, string sourceBranch, string destBranch, string currencyId, string baseAmount, string IBTClearingType, string referenceId = "");

}
