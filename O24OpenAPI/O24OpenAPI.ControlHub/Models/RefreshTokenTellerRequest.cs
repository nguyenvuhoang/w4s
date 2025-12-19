using O24OpenAPI.Core.Abstractions;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.ControlHub.Models;

public class RefreshTokenTellerRequest : BaseTransactionModel
{
    public string CoreToken { get; set; }
    public string RefreshToken { get; set; }
    public string OldRefreshToken { get; set; }
}
