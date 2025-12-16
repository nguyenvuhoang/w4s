using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CMS;
using O24OpenAPI.Framework.Controllers;

namespace O24OpenAPI.Web.Report.Controllers;

public class TestController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Test(string token)
    {
        var userSession = await EngineContext
            .Current.Resolve<ICMSGrpcClientService>()
            .GetUserSessionAsync(token);
        return Ok(userSession);
    }
}
