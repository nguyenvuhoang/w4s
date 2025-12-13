using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class TransitionUserStatusModel : BaseTransactionModel
{
    public string ContractNumber { get; set; }
    public new string Status { get; set; }
}
