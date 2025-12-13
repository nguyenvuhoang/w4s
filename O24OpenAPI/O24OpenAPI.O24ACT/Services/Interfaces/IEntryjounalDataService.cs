using O24OpenAPI.O24ACT.Models;
using O24OpenAPI.O24ACT.Models.Response;
using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.O24ACT.Services.Interfaces;

public interface IEntryjounalDataService
{
    Task<string> GetGLAccountCommonFormat(string accountName, string language = "en");
    Task<string> ChangeGLFormatFromUserCurrency(string accountNumberFormat, string branchCode, string currencyCode);
    Task<EntryPostingReponse> ExcuteEntryPosting(BaseTransactionModel message);
    Task UpdatePosting(BaseTransactionModel message, AccountingRuleProccessModel entryDefine);
}
