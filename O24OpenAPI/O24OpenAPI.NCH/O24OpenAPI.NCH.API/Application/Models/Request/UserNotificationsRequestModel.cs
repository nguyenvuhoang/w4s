using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.NCH.API.Application.Models.Request;

public class UserNotificationsRequestModel : BaseTransactionModel
{
    public string UserCode { get; set; }
    public string Category { get; set; }
}
