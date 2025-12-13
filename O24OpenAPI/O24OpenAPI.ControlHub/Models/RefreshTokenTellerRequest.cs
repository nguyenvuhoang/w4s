using O24OpenAPI.Web.Framework.Models;

namespace O24OpenAPI.ControlHub.Models.Request;

public class RefreshTokenTellerRequest : BaseTransactionModel
{
    public string CoreToken { get; set; }
    public string RefreshToken { get; set; }
    public string OldRefreshToken { get; set; }
}
