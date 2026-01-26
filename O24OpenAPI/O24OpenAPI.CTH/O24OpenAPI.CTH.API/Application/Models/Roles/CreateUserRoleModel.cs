using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Models.Roles;

public class CreateUserRoleModel : BaseTransactionModel
{
    public string RoleName { get; set; }
    public string RoleType { get; set; }
    public string ServiceID { get; set; }
    public string RoleDescription { get; set; }
}
