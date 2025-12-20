using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Roles;

public class ModelWithRoleId : BaseSearch
{
    public int RoleId { get; set; }
}
