using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.Request;

namespace O24OpenAPI.Web.CMS.Services.Interfaces;

public interface ICoreAuthenticateService
{
    public Task<LoginCoreResponse> Login(AuthenJWTModel model);
    Task<string> Logout();
    Task<JToken> ChangePassword(ChangePasswordModel model);
}
