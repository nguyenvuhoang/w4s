using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class DeleteUserContractModel : BaseTransactionModel
{
    public string ContractNumber { get; set; }
}
