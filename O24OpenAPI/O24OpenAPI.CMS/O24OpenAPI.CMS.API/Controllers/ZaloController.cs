using LinKit.Core.Cqrs;
using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CMS.API.Application.Features.Zalo;
using O24OpenAPI.CMS.API.Models.Zalo;
using O24OpenAPI.Framework.Controllers;

namespace O24OpenAPI.CMS.API.Controllers;

public class ZaloController([FromKeyedServices(MediatorKey.CMS)] IMediator mediator) : BaseController
{

    [HttpPost]
    public virtual async Task<IActionResult> Create(
        [FromBody] GenerateAccessTokenCommand request,
        CancellationToken cancellationToken
    )
    {
        var result = await mediator.SendAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
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
