using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Request;

public class RefreshTokenRequest : BaseTransactionModel
{
    public string RefreshToken { get; set; }
    public string DeviceId { get; set; }
}
