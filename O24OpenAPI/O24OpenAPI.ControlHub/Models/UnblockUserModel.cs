using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class UnblockUserModel : BaseTransactionModel
{
    public string UserName { get; set; }
}
