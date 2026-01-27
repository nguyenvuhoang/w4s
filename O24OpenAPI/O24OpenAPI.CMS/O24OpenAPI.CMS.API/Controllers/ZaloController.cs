using LinKit.Core.Cqrs;
using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CMS.API.Application.Features.Zalo;
using O24OpenAPI.CMS.API.Models.Zalo;
using O24OpenAPI.Framework.Controllers;

namespace O24OpenAPI.CMS.API.Controllers;

public class ZaloController([FromKeyedServices(MediatorKey.CMS)] IMediator mediator) : BaseController
{

    [HttpGet]
    public virtual async Task<IActionResult> Create(
        CancellationToken cancellationToken
    )
    {
        var request = new GenerateAccessTokenCommand();
        var result = await mediator.SendAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("/api/zalo/return")]
    public async Task<IActionResult> Return([FromQuery] ZaloProcessModel request, CancellationToken ct)
    {
        var cmd = new ZaloExchangeTokenCommand
        {
            OaId = request.OaId ?? "",
            Code = request.Code ?? "",
            State = request.State ?? ""
        };

        var result = await mediator.SendAsync(cmd, ct);
        return Ok(result);
    }

}
