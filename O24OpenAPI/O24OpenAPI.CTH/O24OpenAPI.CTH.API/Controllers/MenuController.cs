using LinKit.Core.Cqrs;
using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.CTH.API.Application.Features.UserCommands;
using O24OpenAPI.Framework.Controllers;

namespace O24OpenAPI.CTH.API.Controllers;

public class MenuController([FromKeyedServices(MediatorKey.CTH)] IMediator mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Load(
        [FromBody] LoadFullUserCommandsQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var result = await mediator.QueryAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            throw ex.InnerException;
        }
    }
}
