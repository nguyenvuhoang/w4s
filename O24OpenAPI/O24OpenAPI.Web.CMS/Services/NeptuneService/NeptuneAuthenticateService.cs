using Newtonsoft.Json.Linq;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Web.CMS.Models.Request;
using O24OpenAPI.Web.CMS.Services.Interfaces;

namespace O24OpenAPI.Web.CMS.Services.NeptuneService;

public class NeptuneAuthenticateService : ICoreAuthenticateService
{
    public Task<JToken> ChangePassword(ChangePasswordModel model)
    {
        throw new NotImplementedException();
    }

    public Task<LoginCoreResponse> Login(AuthenJWTModel model)
    {
        throw new NotImplementedException();
    }

    public Task<string> Logout()
    {
        throw new NotImplementedException();
    }
}
