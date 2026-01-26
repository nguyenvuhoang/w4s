using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Models.Roles;

public class UserRightUpdateModel : BaseTransactionModel
{
    public List<UserRightModel> ListUserRight { get; set; }
}
