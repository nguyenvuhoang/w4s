using O24OpenAPI.CTH.API.Application.Models.Roles;

namespace O24OpenAPI.CTH.API.Application.Models;

public class UpdateUserInRoleModel : ModelWithRoleId
{
    public List<string> ListUser { get; set; }
    public string Action { get; set; }
}
