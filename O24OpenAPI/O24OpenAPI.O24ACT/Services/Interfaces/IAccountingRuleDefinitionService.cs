using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.O24ACT.Models;

namespace O24OpenAPI.O24ACT.Services.Interfaces;

public interface IAccountingRuleDefinitionService
{
    AccountingRuleDefinitionModel GeneratedPostingTemporory(BaseTransactionModel model);
}
