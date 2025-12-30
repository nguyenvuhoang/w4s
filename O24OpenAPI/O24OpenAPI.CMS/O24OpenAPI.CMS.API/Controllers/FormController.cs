using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.Framework.Controllers;

namespace O24OpenAPI.CMS.API.Controllers;

public class FormController(ILoadFormService formService) : BaseController
{
    private readonly ILoadFormService _formService = formService;
    [HttpPost]
    public virtual async Task<IActionResult> Load([FromBody] FormModelRequest request)
    {
        await _formService.LoadFormAndRoleTask(request);
        return Ok(request);
    }
}
