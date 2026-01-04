using O24OpenAPI.Framework.Models;
using O24OpenAPI.ACT.API.Application.Models.Response;
using O24OpenAPI.ACT.API.Application.Models;

namespace O24OpenAPI.ACT.API.Application.Services;

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
