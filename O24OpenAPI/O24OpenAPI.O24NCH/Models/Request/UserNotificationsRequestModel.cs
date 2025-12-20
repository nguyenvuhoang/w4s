using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.O24NCH.Models.Request;

public class UserNotificationsRequestModel : BaseTransactionModel
{
    public string UserCode { get; set; }
    public string Category { get; set; }
}
