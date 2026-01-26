using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Models.Roles;

public class UpdateUserRoleModel : BaseTransactionModel
{
    public int RoleId { get; set; }
    public List<string> ListOfUser { get; set; } = [];
    public bool IsAssign { get; set; }
}
