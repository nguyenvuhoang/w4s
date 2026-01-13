using O24OpenAPI.Framework.Models;
using O24OpenAPI.O24ACT.Models;
using O24OpenAPI.O24ACT.Models.Response;

namespace O24OpenAPI.O24ACT.Services.Interfaces;

public interface IEntryjounalDataService
{
    Task<string> GetGLAccountCommonFormat(string accountName, string language = "en");
    Task<string> ChangeGLFormatFromUserCurrency(
        string accountNumberFormat,
        string branchCode,
        string currencyCode
    );
    Task<EntryPostingReponse> ExcuteEntryPosting(BaseTransactionModel message);
    Task UpdatePosting(BaseTransactionModel message, AccountingRuleProccessModel entryDefine);
}
