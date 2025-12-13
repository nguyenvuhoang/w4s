using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.O24ACT.Services.Interfaces;

public partial interface ITransactionRulesService
{
    Task Validate(BaseTransactionModel model, object data);
}
