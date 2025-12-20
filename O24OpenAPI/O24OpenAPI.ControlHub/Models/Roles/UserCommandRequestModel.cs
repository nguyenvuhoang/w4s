using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Roles;

public class UserCommandRequestModel : BaseTransactionModel
{
    public string CommandId { get; set; }
    public string Channel { get; set; } = string.Empty;
}
