using O24OpenAPI.ControlHub.Models.Roles;

namespace O24OpenAPI.ControlHub.Models.Portal;

public class UpdateUserInRoleModel : ModelWithRoleId
{
    public List<string> ListUser { get; set; }
    public string Action { get; set; }
}
