using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class TransitionUserStatusModel : BaseTransactionModel
{
    public string ContractNumber { get; set; }
    public new string Status { get; set; }
}
