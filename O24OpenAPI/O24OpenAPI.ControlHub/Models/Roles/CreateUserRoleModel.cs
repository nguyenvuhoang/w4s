using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Roles;

public class CreateUserRoleModel : BaseTransactionModel
{
    public string RoleName { get; set; }
    public string RoleType { get; set; }
    public string ServiceID { get; set; }
    public string RoleDescription { get; set; }
}
