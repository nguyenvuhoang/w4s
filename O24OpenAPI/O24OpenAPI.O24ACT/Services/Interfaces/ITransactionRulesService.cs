using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24ACT.Services.Interfaces;

public partial interface ITransactionRulesService
{
    Task Validate(BaseTransactionModel model, object data);
}
