using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Roles;

public class UserRightUpdateModel : BaseTransactionModel
{
    public List<UserRightModel> ListUserRight { get; set; }
}
