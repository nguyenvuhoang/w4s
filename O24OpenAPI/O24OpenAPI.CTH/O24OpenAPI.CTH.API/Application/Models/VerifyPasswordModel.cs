using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Models;

public class VerifyPasswordModel : BaseTransactionModel
{
    public string Password { get; set; }
    public string UserCode { get; set; }
}
