using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class UpdateUserAvatarRequestModel : BaseTransactionModel
{
    public string UserCode { get; set; }
    public string AvatarUrl { get; set; }
}
