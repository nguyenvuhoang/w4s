using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class DeleteUserContractModel : BaseTransactionModel
{
    public string ContractNumber { get; set; }
}
