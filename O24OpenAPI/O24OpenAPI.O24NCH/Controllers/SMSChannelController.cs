using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Client.Enums;
using O24OpenAPI.Core.Infrastructure;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services;
using O24OpenAPI.O24NCH.Models.Request.SMS;
using O24OpenAPI.O24NCH.Services.Interfaces;

namespace O24OpenAPI.O24NCH.Controllers;

public partial class SMSChannelController : BaseController
{
    private readonly ISMSService _smsService = EngineContext.Current.Resolve<ISMSService>();

    /// <summary>
    /// Post the workflow step from CMS workflow scheme
    /// </summary>
    /// <param name="CrossServiceRequest">The workflow scheme</param>
    [HttpPost]
    public virtual async Task<IActionResult> Post(
        [FromBody] CrossServiceRequest crossServiceRequest
    )
    {
        if (crossServiceRequest.ProcessNumber != ProcessNumber.ExecuteCommand)
        {
            return Ok(
                await BaseQueueService.InvokeCommandQuery(crossServiceRequest.WorkflowScheme)
            );
        }
        var response = await BaseQueueService.InvokeAsync(
            crossServiceRequest.WorkflowScheme,
            crossServiceRequest.FullClassName,
            crossServiceRequest.MethodName,
            "O24OpenAPI.O24NCH"
        );
        return Ok(response);
    }

    [HttpPost]
    public virtual async Task<IActionResult> SendSMS([FromBody] SMSGatewayRequestModel smsSendModel)
    {
        var response = await _smsService.SMSGatewaySend(smsSendModel);
        return Ok(response);
    }

    [HttpGet]
    public IActionResult GenerateOtpHash([FromQuery] string otp)
    {
        if (string.IsNullOrWhiteSpace(otp))
        {
            return BadRequest("OTP is required");
        }

        var hash = Utils.Utility.EncryptOTP(otp);
        return Ok(hash);
    }
}
