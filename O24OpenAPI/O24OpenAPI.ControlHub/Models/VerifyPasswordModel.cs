using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class VerifyPasswordModel : BaseTransactionModel
{
    public string Password { get; set; }
    public string UserCode { get; set; }
}
