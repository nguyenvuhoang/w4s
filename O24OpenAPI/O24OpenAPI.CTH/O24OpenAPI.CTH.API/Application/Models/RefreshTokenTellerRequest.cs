using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Models;

public class RefreshTokenTellerRequest : BaseTransactionModel
{
    public string CoreToken { get; set; }
    public string RefreshToken { get; set; }
    public string OldRefreshToken { get; set; }
}
