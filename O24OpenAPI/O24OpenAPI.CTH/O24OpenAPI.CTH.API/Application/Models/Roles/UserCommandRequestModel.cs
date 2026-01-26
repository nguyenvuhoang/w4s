using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Models.Roles;

public class UserCommandRequestModel : BaseTransactionModel
{
    public string CommandId { get; set; }
    public string Channel { get; set; } = string.Empty;
}
