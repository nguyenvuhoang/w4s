using LinKit.Core.Cqrs;
using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.NCH.API.Application.Features.Zalo;

namespace O24OpenAPI.NCH.API.Controllers;


public class ZaloOtpController([FromKeyedServices(MediatorKey.NCH)] IMediator mediator) : BaseController
{
    [HttpPost]
    public virtual async Task<IActionResult> Send(
        [FromBody] SendZnsOtpCommand request,
        CancellationToken cancellationToken
    )
    {
        var result = await mediator.SendAsync(request, cancellationToken);
        return Ok(result);
    }

}
