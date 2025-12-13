using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models.Request;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public partial interface IAuthenticateService
{
    Task<JToken> AuthenJwt(AuthenJWTModel model, long transactionDate, string ip);
    Task<JToken> DigitalHashPassword(WorkflowScheme workflow);
    Task<JToken> Logout(bool isLockoutOldSession = false);
}
