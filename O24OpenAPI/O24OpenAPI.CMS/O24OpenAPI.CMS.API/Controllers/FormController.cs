using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.APIContracts.Models.CTH;
using O24OpenAPI.CMS.API.Application.Services.Interfaces;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.Framework.Extensions;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.GrpcContracts.GrpcClientServices.CTH;

namespace O24OpenAPI.CMS.API.Controllers;

public class FormController(ILoadFormService formService, IJwtTokenService jwtTokenService, ICTHGrpcClientService cthGrpcClientService, WorkContext workflowContext) : BaseController
{
    private readonly ILoadFormService _formService = formService;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly ICTHGrpcClientService _cthGrpcClientService = cthGrpcClientService;
    private readonly WorkContext _workflowContext = workflowContext;

    [HttpPost]
    public async Task<IActionResult> Load([FromBody] FormModelRequest request)
    {
        var token = Request.Headers.Authorization.FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(token))
        {
            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                token = token["Bearer ".Length..].Trim();

            var validate = _jwtTokenService.ValidateToken(token);

            if (!validate.IsValid)
                return Unauthorized(new { message = "invalid.token" });
        }

        CTHUserSessionModel currentSession;
        try
        {
            currentSession = await _cthGrpcClientService.GetUserSessionAsync(token);

            if (currentSession == null)
            {
                return Unauthorized(new
                {
                    error = "session_not_found",
                    message = "User session is invalid"
                });
            }

            _workflowContext.UserContext.SetLoginName(currentSession.LoginName);
            _workflowContext.UserContext.SetUserName(currentSession.UserName);
            _workflowContext.UserContext.SetUserCode(currentSession.UserCode);

        }
        catch (Exception ex)
        {
            await ex.LogErrorAsync();

            return StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                error = "cth_unavailable",
                message = "Can not connect to Control Hub"
            });
        }

        var response = await _formService.LoadFormAndRoleTask(request);
        return Ok(response);
    }

}
