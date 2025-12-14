using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.User;

public class UserBannerModel : BaseTransactionModel
{
    public string UserCode { get; set; }
    public string Banner { get; set; } = string.Empty;
}
