using Microsoft.AspNetCore.Mvc;
using O24OpenAPI.Kit.Keyvault.Extensions;
using O24OpenAPI.Kit.Signature.Extensions;
using O24OpenAPI.Web.CMS.Models;
using O24OpenAPI.Framework.Controllers;

namespace O24OpenAPI.Web.CMS.Controllers;

public class SignatureController : BaseController
{
    [HttpPost]
    public virtual async Task<IActionResult> GenerateSignature([FromBody] BoRequestModel requestJson)
    {
        if (requestJson == null)
        {
            return BadRequest("Request body is required.");
        }

        var privateKey = KeyvaultExtension.GetSecretKey();
        var result = SignatureExtension.GetSignature(requestJson, privateKey);
        var (signature, timestamp, nounce) = await Task.FromResult(result);

        return Ok(new { Signature = signature, Timestamp = timestamp, Nounce = nounce });
    }

}
