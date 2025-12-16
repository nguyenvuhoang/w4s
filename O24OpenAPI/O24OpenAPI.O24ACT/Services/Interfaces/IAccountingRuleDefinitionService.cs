using O24OpenAPI.O24ACT.Models;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24ACT.Services.Interfaces;

public interface IAccountingRuleDefinitionService
{
    AccountingRuleDefinitionModel GeneratedPostingTemporory(BaseTransactionModel model);
}
