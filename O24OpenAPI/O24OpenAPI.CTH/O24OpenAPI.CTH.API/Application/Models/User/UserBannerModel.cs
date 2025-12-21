using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Models.User;

public class UserBannerModel : BaseTransactionModel
{
    public string UserCode { get; set; }
    public string Banner { get; set; } = string.Empty;
}
