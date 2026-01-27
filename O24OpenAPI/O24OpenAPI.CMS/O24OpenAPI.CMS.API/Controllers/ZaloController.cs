using LinKit.Core.Cqrs;
using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CMS.API.Models.Zalo;
using O24OpenAPI.Framework.Controllers;
namespace O24OpenAPI.CMS.API.Controllers;

public class ZaloController([FromKeyedServices(MediatorKey.CMS)] IMediator mediator) : BaseController
{

    [HttpPost("/api/zalo/create")]
    public virtual async Task<IActionResult> Create(
        [FromBody] ZaloProcessModel request,
        CancellationToken cancellationToken
    )
    {
        APIContracts.Models.PMT.ZaloTokenResponseModel result = await mediator.SendAsync(request.ToZaloExchangeTokenCommand(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("/api/zalo/return")]
    public virtual async Task<IActionResult> Return(
        [FromQuery] ZaloProcessModel request,
        CancellationToken cancellationToken
    )
    {
        string oaid = request.OaId ?? string.Empty;
        string code = request.Code ?? string.Empty;

        return Ok();
    }

}
