using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Models;

public class UpdateUserAvatarRequestModel : BaseTransactionModel
{
    public string UserCode { get; set; }
    public string AvatarUrl { get; set; }
}
