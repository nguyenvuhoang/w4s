using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Models;

public class DeleteUserContractModel : BaseTransactionModel
{
    public string ContractNumber { get; set; }
}
