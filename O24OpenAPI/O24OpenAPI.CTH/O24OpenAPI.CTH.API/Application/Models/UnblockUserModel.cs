using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Models;

public class UnblockUserModel : BaseTransactionModel
{
    public string UserName { get; set; }
}
