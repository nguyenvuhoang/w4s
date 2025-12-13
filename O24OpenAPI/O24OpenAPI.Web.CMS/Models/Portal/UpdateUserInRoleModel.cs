namespace O24OpenAPI.Web.CMS.Models.Portal;

public class UpdateUserInRoleModel : ModelWithRoleId
{
    public List<string> ListUser { get; set; }
    public string Action { get; set; }
}
