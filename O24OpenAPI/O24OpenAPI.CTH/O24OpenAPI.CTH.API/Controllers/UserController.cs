using LinKit.Core.Cqrs;
using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.CTH.API.Application.Features.User;
using O24OpenAPI.Framework.Controllers;

namespace O24OpenAPI.CTH.API.Controllers;

public class UserController([FromKeyedServices("cth")] IMediator mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> CreateSupperAdmin(
        [FromBody] CreateSupperAdminCommand request,
        CancellationToken cancellationToken
    )
    {
        bool? result = await mediator.SendAsync(request, cancellationToken);
        return Ok(result);
    }
}
