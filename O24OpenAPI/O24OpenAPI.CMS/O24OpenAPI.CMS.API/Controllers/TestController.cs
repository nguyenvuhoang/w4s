using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CTH;

namespace O24OpenAPI.CMS.API.Controllers;

public class TestController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> TestGrpc()
    {
        ICTHGrpcClientService cth = EngineContext.Current.Resolve<ICTHGrpcClientService>();
        CTHUserSessionModel s = await cth.GetUserSessionAsync("x");
        return Ok(s);
    }
}
