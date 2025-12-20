using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class ChangeOwnerRequestModel : BaseTransactionModel
{
    public ChangeOwnerRequestModel() { }

    public string OldPassword { get; set; }
    public string Password { get; set; }
}
