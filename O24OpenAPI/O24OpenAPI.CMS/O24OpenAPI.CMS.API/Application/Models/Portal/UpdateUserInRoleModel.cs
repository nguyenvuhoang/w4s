namespace O24OpenAPI.CMS.API.Application.Models.Portal;

public class UpdateUserInRoleModel : ModelWithRoleId
{
    public List<string> ListUser { get; set; }
    public string Action { get; set; }
}
