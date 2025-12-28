using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Client.Enums;
using O24OpenAPI.Framework.Controllers;
using O24OpenAPI.Framework.Models;
using O24OpenAPI.Framework.Services.Queue;
using System.Security.Cryptography;
using System.Text;

namespace O24OpenAPI.NCH.Controllers;

public partial class SMSChannelController : BaseController
{
    /// <summary>
    /// Post the workflow step from CMS workflow scheme
    /// </summary>
    /// <param name="CrossServiceRequest">The workflow scheme</param>
    [HttpPost]
    public virtual async Task<IActionResult> Post([FromBody] CrossServiceRequest crossServiceRequest)
    {
        if (crossServiceRequest.ProcessNumber != ProcessNumber.ExecuteCommand)
        {
            return Ok(await BaseQueueService.InvokeCommandQuery(crossServiceRequest.WorkflowScheme));
        }

        var response = await BaseQueueService.InvokeAsync(
            crossServiceRequest.WorkflowScheme,
            crossServiceRequest.FullClassName,
            crossServiceRequest.MethodName,
            "O24OpenAPI.NCH"
        );
        return Ok(response);
    }

    [HttpGet]
    public IActionResult GenerateOtpHash([FromQuery] string otp)
    {
        if (string.IsNullOrWhiteSpace(otp))
        {
            return BadRequest("OTP is required");
        }

        // Bản rút gọn: hash SHA256 để không phụ thuộc Utils.Utility
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(otp));
        var hash = Convert.ToHexString(bytes); // uppercase hex
        return Ok(hash);
    }
}
