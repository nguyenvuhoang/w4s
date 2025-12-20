using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Roles;

public class UpdateUserRoleModel : BaseTransactionModel
{
    public int RoleId { get; set; }
    public List<string> ListOfUser { get; set; } = [];
    public bool IsAssign { get; set; }
}
