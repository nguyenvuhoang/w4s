using LinKit.Core.Cqrs;
using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.Core;
using O24OpenAPI.CTH.API.Application.Features.UserCommands;
using O24OpenAPI.CTH.API.Application.Models.Roles;
using O24OpenAPI.CTH.API.Application.Models.UserCommandModels;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Controllers;

public class MenuController([FromKeyedServices(MediatorKey.CTH)] IMediator mediator)
    : BaseController
{
    /// <summary>
    /// Load user commands with full details
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Load([FromBody] LoadFullUserCommandsQuery request)
    {
        IPagedList<CTHUserCommandModel> result = await mediator.QueryAsync(request);
        if (result == null)
        {
            return NotFound();
        }

        PagedListModel<CTHUserCommandModel, MenuInfoModel> response = result.ToPagedListModel<
            CTHUserCommandModel,
            MenuInfoModel
        >();

        return Ok(response);
    }

    /// <summary>
    /// Create Menu Command
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateMenuCommand request,
        CancellationToken cancellationToken
    )
    {
        UserCommandResponseModel result = await mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Modify Menu Command
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Modify(
        [FromBody] ModifyMenuCommand request,
        CancellationToken cancellationToken
    )
    {
        UserCommandResponseModel result = await mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Delete Menu Command
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Delete(
        [FromBody] DeleteMenuCommand request,
        CancellationToken cancellationToken
    )
    {
        bool result = await mediator.SendAsync(request, cancellationToken);

        return Ok(result);
    }
}
