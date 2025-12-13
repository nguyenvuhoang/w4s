using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Roles;

public class UserCommandRequestModel : BaseTransactionModel
{
    public string CommandId { get; set; }
    public string Channel { get; set; } = string.Empty;
}
