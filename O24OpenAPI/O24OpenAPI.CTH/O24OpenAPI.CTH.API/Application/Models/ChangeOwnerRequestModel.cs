using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Models;

public class ChangeOwnerRequestModel : BaseTransactionModel
{
    public ChangeOwnerRequestModel() { }

    public string OldPassword { get; set; }
    public string Password { get; set; }
}
