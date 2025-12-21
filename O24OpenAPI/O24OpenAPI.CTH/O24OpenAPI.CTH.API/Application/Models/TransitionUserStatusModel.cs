using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Models;

public class TransitionUserStatusModel : BaseTransactionModel
{
    public string ContractNumber { get; set; }
    public new string Status { get; set; }
}
