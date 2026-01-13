using O24OpenAPI.O24ACT.Models.Request;
using O24OpenAPI.O24ACT.Models.Response;

namespace O24OpenAPI.O24ACT.Services.Interfaces;

public interface IFOTransactionServices
{
    Task<EntryPostingReponse> ExcuteAccountingRuleAsync(ExcuteAccountingRuleModel model);
}
