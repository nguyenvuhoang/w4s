using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Roles;

public class UpdateUserRoleModel : BaseTransactionModel
{
    public int RoleId { get; set; }
    public List<string> ListOfUser { get; set; } = [];
    public bool IsAssign { get; set; }
}
