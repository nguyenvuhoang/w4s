using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class RefreshTokenRequest : BaseTransactionModel
{
    public string RefreshToken { get; set; }
    public string DeviceId { get; set; }
}
