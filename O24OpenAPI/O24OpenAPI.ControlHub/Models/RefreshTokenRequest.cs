using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Request;

public class RefreshTokenRequest : BaseTransactionModel
{
    public string RefreshToken { get; set; }
    public string DeviceId { get; set; }
}
