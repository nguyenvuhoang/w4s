using LinKit.Core.Cqrs;
using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.CTH.API.Application.Features.UserCommands;
using O24OpenAPI.CTH.API.Application.Models.UserCommandModels;
using O24OpenAPI.Data.System.Linq;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.Framework.Helpers;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Controllers;

public class MenuController([FromKeyedServices(MediatorKey.CTH)] IMediator mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Load(
        [FromBody] SimpleSearchModel request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var search = new LoadFullUserCommandsQuery();

            var result = await mediator.QueryAsync(search, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }

            var pageList = await result.AsQueryable().ToPagedList(request.PageIndex, request.PageSize);
            var response = pageList.ToPagedListModel<CTHUserCommandModel, MenuInfoModel>();

            return Ok(new
            {
                data = response
            });

        }
        catch (Exception ex)
        {
            throw ex.InnerException;
        }
    }
}
